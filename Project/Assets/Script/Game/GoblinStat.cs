using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinStat : MonoBehaviour
{
    public int maxStealth = 10;
    public int currentStealth { get; private set; }

    void Awake()
    {
        currentStealth = Random.Range(0, maxStealth);
    }
}
