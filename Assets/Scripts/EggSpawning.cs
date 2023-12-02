using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggSpawning : MonoBehaviour
{
    public int NumEggs;
    public GameObject BeetleEggs;

    private float minX = -8f;
    private float maxX = 8f;
    private float minY = -4.2f;
    private float maxY = 2.5f;

    public List<GameObject> spawnedEggs = new List<GameObject>();

    //This start fucntion getts random values for the spawn location and instantiaties the Egg objects in the scene
    void Start()
    {
        for (int i = 0; i < NumEggs; i++)
        {
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);

            GameObject spawnedEgg = Instantiate(BeetleEggs, spawnPosition, Quaternion.identity);
            spawnedEggs.Add(spawnedEgg);
            //Debug.Log(spawnPosition);
        }
    }

    // Function to check if all eggs are touched
    public bool AllEggsTouched()
    {
        foreach (GameObject egg in spawnedEggs)
        {
            EggCollision eggCollision = egg.GetComponent<EggCollision>();
            if (eggCollision != null && !eggCollision.found)
            {
                return false; // Return false if any egg is not touched
            }
        }
        return true; // Return true if all eggs are touched
    }
}
