using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildAction", menuName = "hvo/Actions/BuildAction")]
public class BuildActionSO : ActionSO
{


    [SerializeField] private Sprite m_PlacementSprite;
    [SerializeField] private Sprite m_FoundationSprite;
    [SerializeField] private Sprite m_CompletionSprite;
    [SerializeField] private Vector3Int m_BuildingSize;
    [SerializeField] private Vector3Int m_OriginOffset;
    [SerializeField] private int m_GoldCost;
    [SerializeField] private int m_WoodCost;

    /*
        All these fields can be assigned in the unity inspector, that's why its all blank here.
        the functions below are what you see in unity inspector
    */
    public Sprite PlacementSprite => m_PlacementSprite;
    public Sprite FoundationSprite => m_FoundationSprite;
    public Sprite CompletionSprite => m_CompletionSprite;
    public int GoldCost => m_GoldCost;
    public int WoodCost => m_WoodCost;
    public Vector3Int BuildingSize => m_BuildingSize;
    public Vector3Int OriginOffset => m_OriginOffset;
    public override void Execute(GameManager manager)
    {
        manager.StartBuildProcess(this);
    }
}