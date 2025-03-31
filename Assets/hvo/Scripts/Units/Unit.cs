//Units, Humanoid, Enemy, Structures, 
using UnityEngine;
public abstract class Unit: MonoBehaviour{
    [SerializeField] private ActionSO[] m_Actions;

    public bool IsMoving;
    public bool IsTargeted;
    protected Animator m_Animator;
    protected AIPawn m_AIPawn;
    protected SpriteRenderer m_SpriteRenderer;
    protected Material m_OriginalMaterial;
    protected Material m_HighlightMaterial;

    public ActionSO[] Actions => m_Actions;
    protected void Awake()
    {
        m_Animator = GetComponent<Animator>();

        if (TryGetComponent<Animator>(out var animator)){
            m_Animator = animator;
        }

        if (TryGetComponent<AIPawn>(out var aiPawn)){
            m_AIPawn = aiPawn;
        }
        //var manager = GameManager.Get();
        //manager.Test();

        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_OriginalMaterial = m_SpriteRenderer.material;
        m_HighlightMaterial = Resources.Load<Material>("Materials/Outline"); //From the materials folder you want to load outline

    }

    public void MoveTo(Vector3 destination)
    {
        var direction = (destination - transform.position).normalized;  //direction minus current position, normalized
        m_SpriteRenderer.flipX = direction.x < 0; //if direction is smaller than zero or negative then flip to left

        m_AIPawn.SetDestination(destination);
    }

    public void Select(){
        HightlightUnit();
        IsTargeted = true;
    }
    public void DeSelect(){
        UnHightlightUnit();
        IsTargeted = false;
    }

    void HightlightUnit(){
        m_SpriteRenderer.material = m_HighlightMaterial;
    }

    void UnHightlightUnit(){
        m_SpriteRenderer.material = m_OriginalMaterial;

    }

}