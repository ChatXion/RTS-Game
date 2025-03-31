//What are prefabs, prefabs are presets of objects you want to simply recreate
/* Overall Behavior:
When spawned (via your previous Instantiate code):
Appears fully opaque

Exists for m_Duration seconds (default 1s)

For first 90% of time:
Stays fully visible

Last 10% of time:
Fades out smoothly from opaque to transparent

Then:
Self-destructs

This creates a nice click marker effect that:
Shows where player clicked

Persists briefly

Fades away elegantly

Cleans itself up

Setup Requirements:
Must be attached to a GameObject with a SpriteRenderer

SpriteRenderer needs to be assigned in Inspector

Duration can be adjusted in Inspector */


using UnityEngine;
public class PointToClick: MonoBehaviour
{
    [SerializeField] private float m_Duration = 1f;
    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] AnimationCurve m_ScaleCurve;
    private Vector3 m_InitialScale;

    void Start()
    {
      m_InitialScale = transform.localScale;   
    }
    private float m_Timer;

    void Update()
    {
        m_Timer += Time.deltaTime;
        float scaleMultipler = m_ScaleCurve.Evaluate(m_Timer); //Getting the valule from animation curve field in Unity
        transform.localScale = m_InitialScale * scaleMultipler;

      
        if(m_Timer >= m_Duration * .9f ) //last 10% of point to click timer
        {
            float fadeProgress = (m_Timer - m_Duration * 0.9f) / (m_Duration * 0.1f);
            m_SpriteRenderer.color = new Color(1,1,1,1 - fadeProgress);  //r,g,b,a   "a" is the transparancy or fade of the colors
        }

/*         // Calculate fade progress over entire duration
        float fadeProgress = m_Timer / m_Duration;
        // Set color with decreasing alpha (1 to 0 over time)
        m_SpriteRenderer.color = new Color(1, 1, 1, 1 - fadeProgress); */


        if(m_Timer >= m_Duration){
            Destroy(gameObject);
        }
    }
}