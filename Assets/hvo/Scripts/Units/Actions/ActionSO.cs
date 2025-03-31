using UnityEngine;

public abstract class ActionSO: ScriptableObject
{
    public Sprite Icon;
    public string ActionName;
    public string Guid = System.Guid.NewGuid().ToString(); //specifics new GUID for each object

    public abstract void Execute(GameManager manager);
}