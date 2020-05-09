using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightSpawner : MonoBehaviour
{
    private int rand;
    public int maxPerception = 10;
    bool checkResult = false;
    private int[] currentPerceptions = new int[4];

    [SerializeField]
    public Sprite[] Sprite_Pic;

    [SerializeField]
    public GameObject[] Knights;

    private static KnightSpawner instance;

    public static KnightSpawner MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<KnightSpawner>();
            }

            return instance;
        }
    }

    void Awake()
    {
        NextButtonClicked();
    }
    
    public int[] MyCurrentPerceptions
    {
        get
        {
            return currentPerceptions;
        }
    }

    
    public void NextButtonClicked()
    {
        if (UIManager.MyInstance.MyStage <= 11 && checkResult != true)
        {
            for (int i = 0; i < Knights.Length; i++)
            {
                rand = Random.Range(0, Sprite_Pic.Length);
                Knights[i].GetComponent<SpriteRenderer>().sprite = Sprite_Pic[rand];

                if (Sprite_Pic[rand] != null)
                {
                    MyCurrentPerceptions[i] = (Random.Range(0, maxPerception));
                }
                else
                {
                    MyCurrentPerceptions[i] = 0;
                }
            }
        }
        else
        {
            UIManager.MyInstance.StageValue();
        }
    }
}
