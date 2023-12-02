using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HostSpawner : MonoBehaviour
{
    public GameObject BeetleEggprefab;
    public GameObject Webprefab;
    public GameObject Caterpillarprefab;
    public GameObject Aphidprefab;

    [SerializeField]
    Vector3[] beetleEggSpawnpoints;
    [SerializeField]
    Vector3[] webSpawnpoints;
    [SerializeField]
    Vector3[] aphidSpawnpoints;
    [SerializeField]
    Vector3[] caterpillarSpawnpoints;

    //Stores all apswned hosts
    public Dictionary<string, Vector3> hostsData = new Dictionary<string, Vector3>(); // Used for sending data to game manager
    public List<GameObject> hosts = new List<GameObject>(); // used for detecting hosts for parasitzation button

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.spawnedHostsData.Count == 0)
        {
            //initial load of hosts
            //TestSpawn();
            SpawnHosts();
        }
        else
        {
            //Reload the hosts from game manager here.
            GameManager.Instance.currentParaAttempts += 1;
            if (GameManager.Instance.currentParaAttempts == GameManager.Instance.parasitizationAttempts)
            {
                EndRound();
            }
            ReloadHosts();
        }
    }

    void TestSpawn()
    {
        GameManager.Instance.currentParaAttempts = 0;
        Vector3 spawnPosition = new Vector3(-10, -21f, 0);
        Quaternion spawnRotation = Quaternion.identity;
        GameObject beetleEgg = GameObject.Instantiate(BeetleEggprefab, spawnPosition, spawnRotation);

        spawnPosition = new Vector3(10, -21f, 0);
        GameObject web = GameObject.Instantiate(Webprefab, spawnPosition, spawnRotation);

        spawnPosition = new Vector3(30, -21, 0);
        GameObject aphid = GameObject.Instantiate(Aphidprefab, spawnPosition, spawnRotation);

        spawnPosition = new Vector3(-30, -21, 0);
        GameObject caterpillar = GameObject.Instantiate(Caterpillarprefab, spawnPosition, spawnRotation);


        hosts.Add(beetleEgg);
        hosts.Add(web);
        hosts.Add(aphid);
        hosts.Add(caterpillar);

        hostsData.Add("beetleEgg", beetleEgg.transform.position);
        hostsData.Add("web", web.transform.position);
        hostsData.Add("aphid", aphid.transform.position);
        hostsData.Add("caterpillar", caterpillar.transform.position);
        SaveHostsInGameManager();
    }

    void SaveHostsInGameManager()
    {
        GameManager.Instance.spawnedHostsData = hostsData;
    }

    void SpawnHosts()
    {
        GameManager.Instance.currentParaAttempts = 0;
        Vector3 spawnPosition = new Vector3(0, 0, 0);
        Quaternion spawnRotation = Quaternion.identity;

        int randomIndex = Random.Range(0, beetleEggSpawnpoints.Length);
        spawnPosition = beetleEggSpawnpoints[randomIndex];
        GameObject beetleEgg = GameObject.Instantiate(BeetleEggprefab, spawnPosition, spawnRotation);

        randomIndex = Random.Range(0, webSpawnpoints.Length);
        spawnPosition = webSpawnpoints[randomIndex];
        GameObject web = GameObject.Instantiate(Webprefab, spawnPosition, spawnRotation);

        randomIndex = Random.Range(0, aphidSpawnpoints.Length);
        spawnPosition = aphidSpawnpoints[randomIndex];
        GameObject aphid = GameObject.Instantiate(Aphidprefab, spawnPosition, spawnRotation);

        randomIndex = Random.Range(0, caterpillarSpawnpoints.Length);
        spawnPosition = caterpillarSpawnpoints[randomIndex];
        GameObject caterpillar = GameObject.Instantiate(Caterpillarprefab, spawnPosition, spawnRotation);

        hosts.Add(beetleEgg);
        hosts.Add(web);
        hosts.Add(aphid);
        hosts.Add(caterpillar);

        hostsData.Add("beetleEggGW", beetleEgg.transform.position);
        hostsData.Add("web", web.transform.position);
        hostsData.Add("aphid", aphid.transform.position);
        hostsData.Add("caterpillar", caterpillar.transform.position);
        SaveHostsInGameManager();
    }

    void EndRound()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
    }

    void ReloadHosts()
    {
        //Spawns new hosts based on saved host name and host transform.
        Quaternion spawnRotation = Quaternion.identity;
        foreach (KeyValuePair<string, Vector3> kvp in GameManager.Instance.spawnedHostsData)
        {
            switch (kvp.Key)
            {
                case "beetleEgg":
                    GameObject beetleEgg = GameObject.Instantiate(BeetleEggprefab, kvp.Value, spawnRotation);
                    hosts.Add(beetleEgg);
                    break;
                case "aphid":
                    GameObject aphid = GameObject.Instantiate(Aphidprefab, kvp.Value, spawnRotation);
                    hosts.Add(aphid);
                    break;
                case "web":
                    GameObject web = GameObject.Instantiate(Webprefab, kvp.Value, spawnRotation);
                    hosts.Add(web);
                    break;
                case "caterpillar":
                    GameObject caterpillar = GameObject.Instantiate(Caterpillarprefab, kvp.Value, spawnRotation);
                    hosts.Add(caterpillar);
                    break;
            }
        }
    }
}