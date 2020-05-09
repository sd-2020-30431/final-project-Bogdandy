using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinStat : MonoBehaviour
{
    public int maxStealth = 10;
    public int currentStealth { get; private set; }

    private static GoblinStat instance;

    public static GoblinStat MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GoblinStat>();
            }

            return instance;
        }
    }

    void Awake()
    {
        currentStealth = Random.Range(0, maxStealth);
    }
}
