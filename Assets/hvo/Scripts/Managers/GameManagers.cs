using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : SingletonManager<GameManager>
{
    [Header("TileMaps")]
    [SerializeField] private Tilemap m_WalkableTilemap;
    [SerializeField] private Tilemap m_OverlayTilemap;
    [SerializeField] private Tilemap[] m_UnreachableTilemaps;
    [Header("UI")]
    [SerializeField] private PointToClick m_PointToClickPrefab;
    [SerializeField] private ActionBar m_ActionBar;
    [SerializeField] private ConfirmationBar m_BuildConfirmationBar;

    public Unit ActiveUnit; //Selected Active unit, calls upon the Unit.cs we created
    public bool HasActiveUnit => ActiveUnit != null; //HasActiveUnit will be true if Activeunit does not equal null
    
    private PlacementProcess m_PlacementProcess;
    private int m_Gold = 1000;
    private int m_Wood = 1000;

    public int Gold => m_Gold;
    public int Wood => m_Wood;

    /* Block of code moved to HVOUtils.cs 
    public Vector2 InputPosition => Input.touchCount > 0 ? Input.GetTouch(0).position : Input.mousePosition;
    public bool isLeftClickOrTapDown => Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
    public bool isLeftClickorTapUp => Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended);
    */
    public void StartBuildProcess(BuildActionSO buildAction)
    {
        if(m_PlacementProcess != null) return;
        //Debug.Log("Starting Action: " + buildAction.ActionName);
        m_PlacementProcess = new PlacementProcess(buildAction, m_WalkableTilemap, m_OverlayTilemap,m_UnreachableTilemaps);
        m_PlacementProcess.ShowPlacementOutline();
        m_BuildConfirmationBar.Show(buildAction.GoldCost, buildAction.WoodCost);
        m_BuildConfirmationBar.SetupHooks(ConfirmBuildPlacement, CancelBuildPlacement);

    }
    void Start()
    {
        ClearActionBarUI();
    }
    void Update()
    {
        // If placing items via placement process, then handle that
        // else if just click on screen or characters, then handle clicking on characters.
        if (m_PlacementProcess != null)
        {
            m_PlacementProcess.Update();
        }
        else if(HVOUtils.TryGetShortLeftClickPosition(out Vector2 inputPosition))
        {
            DetectClick(inputPosition);
        }


        //Debug.Log(inputPosition);
    }

    void DetectClick(Vector2 inputPosition)
    {
        //If you are over UI Element don't move selected unit
        if (HVOUtils.IsPointerOverUIElement())
        {
            return;
        }
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(inputPosition);
        //Click on unit, interact with game object, need add collider on unit for it to work
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        if (HasClickedOnUnit(hit, out var unit))
        {
            HandleClickedOnUnit(unit);
        }
        else
        {
            HandleClickOnGround(worldPoint);
        }
        HandleClickOnGround(worldPoint);
    }

    bool HasClickedOnUnit(RaycastHit2D hit, out Unit unit)
    {
        if (hit.collider != null && hit.collider.TryGetComponent<Unit>(out var clickedUnit))
        { //basically just says if hit clicked on capsule collider 2D
            unit = clickedUnit;
            return true;
        }
        unit = null;
        return false;
    }
    //Where ever you click on world point activate unit will move there
    void HandleClickOnGround(Vector2 worldPoint)
    {
        if (HasActiveUnit && IsHumanoid(ActiveUnit))
        {
            if (ClickManager.Instance.CanSpawn())
            {
                DisplayClickedEffect(worldPoint);
            }
            if (ActiveUnit != null)
            {
                ActiveUnit.MoveTo(worldPoint);
            }
        }


    }

    void HandleClickedOnUnit(Unit unit)
    {
        if (HasActiveUnit)
        { //if unit is already selected and you click it again, it will deselect it and unhighlight it
            if (HasClickedOnActiveUnit(unit))
            {
                CancelActiveUnit();
                return;
            }
        }
        SelectNewUnit(unit);
    }

    void SelectNewUnit(Unit unit)
    {
        //if active unit already select, unhighlight it and deselect it
        if (HasActiveUnit)
        {
            ActiveUnit.DeSelect();
        }

        ActiveUnit = unit;
        Debug.Log(ActiveUnit.name + " Selected");
        ActiveUnit.Select();
        ShowActionBar(ActiveUnit);
    }
    bool HasClickedOnActiveUnit(Unit clickedUnit)
    {
        return clickedUnit == ActiveUnit;
    }
    void DisplayClickedEffect(Vector2 worldPoint)
    {
        //When called, it spawns a copy of a predefined effect object (m_PointToClickPrefab) at the clicked position in the game world
        //Instantiate() is a Unity function that creates a new instance (copy) of an object
        //(Vector3)worldPoint: Converts the 2D Vector2 to a 3D Vector3 position (z becomes 0 by default)
        //Quaternion.identity: Sets the rotation to no rotation (default orientation)
        Instantiate(m_PointToClickPrefab, (Vector3)worldPoint, Quaternion.identity);

    }
    void CancelActiveUnit()
    {
        ActiveUnit.DeSelect();
        ActiveUnit = null;

        if (m_ActionBar.gameObject.activeSelf)
        {
            ClearActionBarUI();
        }

    }
    bool IsHumanoid(Unit unit)
    {
        return unit is HumanoidUnit;
    }
    void ShowActionBar(Unit unit)
    {
        ClearActionBarUI();
        if (unit.Actions.Length == 0)
        {
            return;
        }


        m_ActionBar.Show();
        foreach (var action in unit.Actions)
        {
            m_ActionBar.RegisterAction(
                action.Icon,
                () => action.Execute(this)
                );
        }
    }
    void ClearActionBarUI()
    {
        m_ActionBar.ClearActions();
        m_ActionBar.Hide();
    }

    void ConfirmBuildPlacement(){
    
        if (TryDeductResources(m_PlacementProcess.GoldCost,m_PlacementProcess.woodCost)){
            if (m_PlacementProcess.TryFinalizePlacement(out Vector3 buildPosition)){
                DeductGold(m_PlacementProcess.GoldCost);
                DeductWood(m_PlacementProcess.woodCost);
                m_BuildConfirmationBar.Hide();
                new BuildingProcess(m_PlacementProcess.BuildAction, buildPosition);  //BUILDS TOWERS
                ActiveUnit.MoveTo(buildPosition);
                ActiveUnit.SetTask(UnitTask.Build);
                m_PlacementProcess = null;
                Debug.Log("Founddations layed out: " + buildPosition);
            }
        }
        else{
            return; 
        }

    }

    /* useless
    void RevertResources(int gold, int wood){
        m_Gold += gold;
        m_Wood += wood;
    }
    */

    void CancelBuildPlacement(){
        m_BuildConfirmationBar.Hide();
        m_PlacementProcess.Cleanup();
        m_PlacementProcess = null;
    }
    public void Test()
    {
        Debug.Log("Button test");
    }

    bool TryDeductResources(int goldCost, int woodCost){
        if(m_Gold >= goldCost && m_Wood >= woodCost){
            return true;
        }
        Debug.Log("Not enough Resources");
        return false;
        
    }

    void DeductWood(int woodCost){
        m_Wood -= woodCost;
    }
    void DeductGold(int goldCost){
        m_Gold -= goldCost;
    }

    //Part of mono behaviors
    void OnGUI()
    {
        GUI.Label(new Rect(100, 40, 200, 20), "Gold: " + m_Gold.ToString(), new GUIStyle { fontSize = 30 });
        GUI.Label(new Rect(100, 80, 200, 20), "Wood: " + m_Wood.ToString(), new GUIStyle { fontSize = 30 });

        if (ActiveUnit != null)
        { 
                    GUI.Label(new Rect(100, 120, 200, 20), "State: " + ActiveUnit.CurrentState.ToString(), new GUIStyle { fontSize = 30 });
                    GUI.Label(new Rect(100, 160, 200, 20), "Task: " + ActiveUnit.CurrentTask.ToString(), new GUIStyle { fontSize = 30 });
        }
    }
}