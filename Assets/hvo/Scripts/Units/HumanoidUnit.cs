
using UnityEngine;

public class HumanoidUnit : Unit
{
    protected UnityEngine.Vector2 m_Velocity;
    protected UnityEngine.Vector3 m_LastPosition;

    public float CurrentSpeed => m_Velocity.magnitude;
    void Start() //fixes position lag
    {
        m_LastPosition = transform.position;
    }
    protected void Update()
    {
        UpdateVelocity();
        UpdatedBehavior();
    }

    //Override in other child classes
    protected virtual void UpdatedBehavior() { }

    protected virtual void UpdateVelocity()
    { 
        //code block checks to see if unit is moving, basically compare current pos
        // with new pos and if there is difference than it is moving. 
        m_Velocity = new UnityEngine.Vector2(
            (transform.position.x - m_LastPosition.x),
            (transform.position.y - m_LastPosition.y)
            ) / Time.deltaTime;
        m_LastPosition = transform.position;
        var state = m_Velocity.magnitude > 0 ? UnitState.Moving : UnitState.Idle; //if greater than zero, then unit state is moving, otherwise it is idle
        SetState(state);

        m_Animator?.SetFloat("Speed", CurrentSpeed);  //? prevents error if object is tower
    }
}


public class EnemyUnit : HumanoidUnit
{

    void Start()
    {

    }
}