
using UnityEngine;
public class WorkerUnit : HumanoidUnit
{
    protected override void UpdatedBehavior()
    {
        CheckForCloseObjects();
    }

    private void CheckForCloseObjects()
    {
        var hits = RunProximityObjectDetection(); //returning us colllider hits
        foreach (var hit in hits)
        {
            if (hit.gameObject == this.gameObject) continue;
            Debug.Log(hit.gameObject.name);
        }
    }
}