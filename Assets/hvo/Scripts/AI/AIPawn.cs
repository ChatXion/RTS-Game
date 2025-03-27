using System;
using UnityEngine;
public class AIPawn : MonoBehaviour
{
    //In C#, [SerializeField] is an attribute in Unity that makes a private field visible and editable in the Unity Inspector, without making it public. 
    // It allows serialization of the field so Unity can save and load its value.

    [SerializeField]
    private float m_speed = 5f;
    private Vector3? m_Destination; //a vector is a struct and cannot be null, unless you add ?, so Vector3? is null

    public Vector3? Destination => m_Destination;

//Moves all units with AIPAWN, testing only
/*     void Start()
    {
        SetDestination(new Vector3(2.89f, 3.45f,0));
    } */


    void Update()
    {
        if(m_Destination.HasValue){
            var direction = m_Destination.Value - transform.position; //how to get vector from destination
            transform.position += direction.normalized * Time.deltaTime * m_speed; //moves character to position based on vector, or sumthing

            var distanceToDestination = Vector3.Distance(transform.position, m_Destination.Value);
            
            if(distanceToDestination < 0.1f){
                m_Destination = null;
            }
        }
    }
    public void SetDestination(Vector3 destination)
    {
        m_Destination = destination;
    }

    public static implicit operator Animator(AIPawn v)
    {
        throw new NotImplementedException();
    }
}