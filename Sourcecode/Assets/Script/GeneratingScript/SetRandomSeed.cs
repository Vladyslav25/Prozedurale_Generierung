using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRandomSeed : MonoBehaviour
{
    public int seed;
    public string stringSeed = "seed string";
    public bool useStringSeed;
    public bool useRandomSeed;

    void Awake()
    {
        if (useStringSeed)
        {
            seed = stringSeed.GetHashCode();
        }
        if (useRandomSeed)
        {
            seed = Random.Range(0, int.MaxValue);
        }
        Random.InitState(seed);
    }
}
