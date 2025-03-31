//CLICK Manager to prevent too many clicks within 1 second

using UnityEngine;
using System.Collections.Generic;

public class ClickManager : MonoBehaviour
{
    public static ClickManager Instance { get; private set; } // Singleton pattern
    private List<float> spawnTimes = new List<float>();      // Tracks spawn timestamps
    private const int MAX_CLICKS_PER_SECOND = 20;            // Maximum allowed clicks
    private const float TIME_WINDOW = 1f;                   // Time window in seconds

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool CanSpawn()
    {
        float currentTime = Time.time;
        // Remove timestamps older than 1 second
        spawnTimes.RemoveAll(t => currentTime - t > TIME_WINDOW);
        
        // Check if we can spawn a new instance
        if (spawnTimes.Count < MAX_CLICKS_PER_SECOND)
        {
            spawnTimes.Add(currentTime);
            return true;
        }
        return false;
    }
}