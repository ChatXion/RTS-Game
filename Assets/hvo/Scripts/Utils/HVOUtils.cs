using UnityEngine;
using UnityEngine.EventSystems;
public static class HVOUtils
{
    public static Vector2 InputPosition => Input.touchCount > 0 ? Input.GetTouch(0).position : Input.mousePosition;
    public static bool isLeftClickOrTapDown => Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
    public static bool isLeftClickorTapUp => Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended);

    public static Vector3 inputHoldWorldPosition => Input.touchCount > 0 ?
             Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position) :
             Input.GetMouseButton(0) ? Camera.main.ScreenToWorldPoint(Input.mousePosition) 
             : Vector2.zero;
    private static Vector2 m_InitialTouchPosition;

    public static bool TryGetShortLeftClickPosition(out Vector2 inputPosition, float maxDistance = 5f){
        inputPosition = InputPosition;
         if (isLeftClickOrTapDown)
            {
                m_InitialTouchPosition = inputPosition;
                //Debug.Log("Here 1");
            }


            //on mouse up or touch up
            //get input pos if click, 0 = left click
            if (isLeftClickorTapUp)
            {
                //swipe or drag distance
                //Debug.Log("Here 2");
                if (Vector2.Distance(m_InitialTouchPosition, inputPosition) < maxDistance)
                {
                    //Debug.Log("Here 3");
                    return true;
                }
            }
            return false;
    }

    public static bool TryGetHoldPosition(out Vector3 worldPosition){
        if(Input.touchCount > 0){
            worldPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            return true;
        }
        else if(Input.GetMouseButton(0)){ //check for left mouse click, which is 0
            worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return true;
        }
        worldPosition = Vector3.zero; //vector3.zero is (0,0,0) in position
        return false;
    }

        public static bool IsPointerOverUIElement()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            return EventSystem.current.IsPointerOverGameObject(touch.fingerId);
        }
        else
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}