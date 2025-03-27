using UnityEngine;

public class GameManager: SingletonManager<GameManager>
{
    public Unit ActiveUnit; //Selected Active unit, calls upon the Unit.cs we created
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
        HandleClickOnGround(worldPoint);
    }

    //Where ever you click on world point activate unit will move there
    void HandleClickOnGround(Vector2 worldPoint){
        if (ActiveUnit == null) {
            Debug.LogError("ActiveUnit is null!");
            return;
        }
        //Debug.Log("Moving ActiveUnit to: " + worldPoint);
        ActiveUnit.MoveTo(worldPoint);
    }
    public void Test(){

    }
}