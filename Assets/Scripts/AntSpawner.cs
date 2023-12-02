// This script controls the spanwing of ants from the spawn points for the Aphid Minigame

using UnityEngine;

public class AntSpawner : MonoBehaviour
{
    public float spawnDelay = 0.3f;
    private float nextTimeToSpawn = 0f;
    public GameObject ant;
    public Transform[] spawnPoints; // must manually create array size on game object
    public Transform[] chosenSpawns; // must manually create array size on game object

    private void Awake()
    {
        ChooseSpawnPoints(chosenSpawns);
    }

    private void Update()
    {
        if (nextTimeToSpawn <= Time.time)
        {
            SpawnAnt();
            nextTimeToSpawn = Time.time + spawnDelay;
        }
    }

    void ChooseSpawnPoints(Transform[] chosenSpawns)
    {
        int j = 0;
        for (int i = 0; i < spawnPoints.Length; i += 2)
        {
            int firstOfSpawnPair = i;
            int secondOfSpawnPair = i + 1;
            int chosenSpawn = Random.Range(firstOfSpawnPair, secondOfSpawnPair + 1); // Random.Range excludes the max number. Added 1 as workaround
            chosenSpawns[j] = spawnPoints[chosenSpawn];
            //Debug.Log("Spawn point chosen: " + chosenSpawn);
            j++;
        }
    }

    void SpawnAnt()
    {
        // picks a random spawn point from the chosenSpawns array
        int randomIndex = Random.Range(0, chosenSpawns.Length);
        Transform spawnPoint = chosenSpawns[randomIndex];
        Instantiate(ant, spawnPoint.position, spawnPoint.rotation);
    }

}
