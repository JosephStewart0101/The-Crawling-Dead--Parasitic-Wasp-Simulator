using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AISpawnData", menuName = "ScriptableAssets/AISpawnData")]
public class AISpawnData : ScriptableObject
{
    [SerializeField]
    private List<Vector2> spawnLocations = new List<Vector2>();

    public List<Vector2> SpawnLocations
    {
        get { return spawnLocations; }
    }

    // Call this to spawn an AI (during set up)
    public void AddSpawnLocation(Vector2 location)
    {
        spawnLocations.Add(location);
    }

    // Call this to remove a spawn location (if an AI dies)
    public void RemoveSpawnLocation(Vector2 location)
    {
        spawnLocations.Remove(location);
    }

    // Call this at the end of the round to clear the spawn data
    public void ClearSpawnLocations()
    {
        spawnLocations.Clear();
    }
}
