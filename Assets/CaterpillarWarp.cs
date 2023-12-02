using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaterpillarWarp : MonoBehaviour
{
    [SerializeField]
    private Transform warpTarget;

    public Transform GetDestination()
    {
        return warpTarget;
    }
}
