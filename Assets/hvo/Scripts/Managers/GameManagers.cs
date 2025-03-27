using UnityEngine;

public class GameManager: SingletonManager<GameManager>
{
    public Unit ActiveUnit; //Selected Active unit, calls upon the Unit.cs we created
    public bool HasActiveUnit => ActiveUnit != null; //HasActiveUnit will be true if Activeunit does not equal null
    private Vector2 m_InitialTouchPosition;
    void Update()
    {
        // takes position from where you click or touch on phone screen.
        Vector2 inputPosition = Input.touchCount > 0 ? Input.GetTouch(0).position : Input.mousePosition;
        
        //on mouse click or touch down
        if(Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)){
            m_InitialTouchPosition = inputPosition;
           //Debug.Log("Here 1");
        } 
        

        //on mouse up or touch up
        //get input pos if click, 0 = left click
        if(Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)){
            //swipe or drag distance
            //Debug.Log("Here 2");
            if(Vector2.Distance(m_InitialTouchPosition, inputPosition) < 10){
                //Debug.Log("Here 3");
                DetectClick(inputPosition); 
            }
        } 
        
        //Debug.Log(inputPosition);
    }

    void DetectClick(Vector2 inputPosition){
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(inputPosition);
        //Click on unit, interact with game object, need add collider on unit for it to work
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero); 
        if(HasClickedOnUnit(hit, out var unit)){
            HandleClickedOnUnit(unit);
        }
        else{
            HandleClickOnGround(worldPoint);
        }
        HandleClickOnGround(worldPoint);
    }

    bool HasClickedOnUnit(RaycastHit2D hit, out Unit unit){
        if(hit.collider != null && hit.collider.TryGetComponent<Unit>(out var clickedUnit)){
            unit = clickedUnit;
            return true;
        }
        unit = null;
        return false;
    }
    //Where ever you click on world point activate unit will move there
    void HandleClickOnGround(Vector2 worldPoint){
        if (ActiveUnit != null){
            ActiveUnit.MoveTo(worldPoint);
        }
    }

    void HandleClickedOnUnit(Unit unit){
        SelectNewUnit(unit);
    }

    void SelectNewUnit(Unit unit){
        //if active unit already select, unhighlight it and deselect it
        if(HasActiveUnit){ 
            ActiveUnit.DeSelect(); 
        }

        ActiveUnit = unit;
        Debug.Log(ActiveUnit.name + " Selected");
        ActiveUnit.Select();
    }
    public void Test(){

    }
}