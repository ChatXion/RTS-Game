

using UnityEngine;

public class BuildingProcess
{
    private BuildActionSO m_BuildAction;
    //where to place foundations of building
    public BuildingProcess(BuildActionSO buildAction, Vector3 placementPosition)
    {
        m_BuildAction = buildAction;
        var structure = Object.Instantiate(buildAction.StructurePrefab);
        //IMPORTANT!! CAN CAUSE UNITY TO CRASH
        structure.Renderer.sprite = m_BuildAction.FoundationSprite;
        structure.transform.position = placementPosition;
        structure.RegisterProcess(this);
    }

    //updates during game run
    public void Update()
    {
        Debug.Log("Building is under construction");
    }
}
