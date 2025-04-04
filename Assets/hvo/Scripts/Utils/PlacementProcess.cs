
using UnityEngine;

public class PlacementProcess{
    private GameObject m_PlacementOutline;
    private BuildActionSO m_BuildAction;
    public PlacementProcess(BuildActionSO buildAction){
        m_BuildAction = buildAction;
    }

 public void Update()
     {
         if (HVOUtils.TryGetHoldPosition(out Vector3 worldPosition))
         {
            m_PlacementOutline.transform.position = new Vector3(worldPosition.x, worldPosition.y, 0);
         }
     }

    public void ShowPlacementOutline()
     {
         Debug.Log("ShowPlacementOutline");
         m_PlacementOutline = new GameObject("PlacementOutline");
         var renderer = m_PlacementOutline.AddComponent<SpriteRenderer>();
         renderer.sortingOrder = 999;
         renderer.color = new Color(1, 1, 1, 0.5f);
         renderer.sprite = m_BuildAction.PlacementSprite;
 
     }
 }
