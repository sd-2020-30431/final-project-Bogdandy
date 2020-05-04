using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightSpawner : MonoBehaviour
{
    private int rand;
    public int maxStealth = 10;
    public int currentStealth { get; private set; }

    [SerializeField]
    public Sprite[] Sprite_Pic;

    void Awake()
    {
        currentStealth = Random.Range(0, maxStealth);
        rand = Random.Range(0, Sprite_Pic.Length);
        GetComponent<SpriteRenderer>().sprite = Sprite_Pic[rand];
    }
    
    public void NextButtonClicked()
    {
        rand = Random.Range(0, Sprite_Pic.Length);
        GetComponent<SpriteRenderer>().sprite = Sprite_Pic[rand];
    }

    public void DoesKnightExist()
    {
        if(GetComponent<SpriteRenderer>().sprite == null)
            Debug.Log("No Knight");
        else
            Debug.Log(this.name);
    }
}
