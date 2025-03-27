//Units, Humanoid, Enemy, Structures, 
using UnityEngine;
public abstract class Unit: MonoBehaviour{
    public bool IsMoving;
    protected Animator m_Animator;
    protected AIPawn m_AIPawn;

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

    }

    public void MoveTo(Vector3 destination)
    {
        m_AIPawn.SetDestination(destination);
    }

}