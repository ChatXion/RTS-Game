//Units, Humanoid, Enemy, Structures, 
using UnityEngine;

public enum UnitState
{
    Idle, Moving, Attacking, Chopping, Mining
}

public enum UnitTask
{ 
    None, Build, Chop, Mine, Attack
}

public abstract class Unit : MonoBehaviour
{
    [SerializeField] private ActionSO[] m_Actions;
    [SerializeField] private float m_ObjectDetectionRadius = 3f;
   
    public bool IsTargeted;
    protected Animator m_Animator;
    protected AIPawn m_AIPawn;
    protected SpriteRenderer m_SpriteRenderer;
    protected Material m_OriginalMaterial;
    protected Material m_HighlightMaterial;

    public UnitState CurrentState { get; protected set; } = UnitState.Idle; //get state, set as protected, default state is idle
    public UnitTask CurrentTask { get; protected set; } = UnitTask.None;
    public ActionSO[] Actions => m_Actions;
    public SpriteRenderer Renderer => m_SpriteRenderer;
    protected void Awake()
    {
        m_Animator = GetComponent<Animator>();

        if (TryGetComponent<Animator>(out var animator))
        {
            m_Animator = animator;
        }

        if (TryGetComponent<AIPawn>(out var aiPawn))
        {
            m_AIPawn = aiPawn;
        }
        //var manager = GameManager.Get();
        //manager.Test();

        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_OriginalMaterial = m_SpriteRenderer.material;
        m_HighlightMaterial = Resources.Load<Material>("Materials/Outline"); //From the materials folder you want to load outline

    }

    public void SetTask(UnitTask task) {
        OnSetTask(CurrentTask, task);
    }
    public void SetState(UnitState state)
    {
        OnSetState(CurrentState, state);
    }

    public void MoveTo(Vector3 destination)
    {
        var direction = (destination - transform.position).normalized;  //direction minus current position, normalized
        m_SpriteRenderer.flipX = direction.x < 0; //if direction is smaller than zero or negative then flip to left

        m_AIPawn.SetDestination(destination);
    }

    public void Select()
    {
        HightlightUnit();
        IsTargeted = true;
    }
    public void DeSelect()
    {
        UnHightlightUnit();
        IsTargeted = false;
    }

    public virtual void OnSetTask(UnitTask oldTask, UnitTask newTask)
    {
        CurrentTask = newTask;
    }

    public virtual void OnSetState(UnitState oldState, UnitState newState)
    {
        CurrentState = newState;
    }

    protected Collider2D[] RunProximityObjectDetection()
    {
        return Physics2D.OverlapCircleAll(transform.position, m_ObjectDetectionRadius);
    }

    void HightlightUnit()
    {
        m_SpriteRenderer.material = m_HighlightMaterial;
    }

    void UnHightlightUnit()
    {
        m_SpriteRenderer.material = m_OriginalMaterial;

    }

    //Monobehavior special function
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 0.3f);
        Gizmos.DrawSphere(transform.position, m_ObjectDetectionRadius);
    }

}