using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    private int goblinStealth = 0;

    private static Game_Manager instance;

    public static Game_Manager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Game_Manager>();
            }

            return instance;
        }
    }

    public void perceptionCheck()
    {
        goblinStealth = GoblinStat.MyInstance.currentStealth;
        int i = 0;

        bool[] results = new bool[4];

        foreach (int knightPercerption in KnightSpawner.MyInstance.MyCurrentPerceptions)
        {
            if(knightPercerption > goblinStealth)
            {
                results[i] = false;
            }
            else
            {
                results[i] = true;
            }
            i++;
        }

        if (hasFailed(results))
        {
            Debug.Log("Goblin Caught");
            FailWindow.MyInstance.RandomResult();
            FailWindow.MyInstance.OpenCloseFailWindow();
        }
        else
        {
            Debug.Log("Goblin Succeded");
            LootWindow.MyInstance.Randomize();
            LootWindow.MyInstance.OpenCloseLootWindow();
        }
    }

    bool hasFailed(bool[] results)
    {
        for (int i = 0; i < results.Length; i++)
        {
            if (!results[i])
            {
                return true;
            }
        }
        return false;
    }
}
