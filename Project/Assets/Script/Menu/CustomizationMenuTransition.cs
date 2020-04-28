using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomizationMenuTransition : MonoBehaviour
{
    public Animator transition;
    public GameObject goblin;
    public List<Customization> customizations = new List<Customization>();

    public void RandomizeGoblin()
    {
        foreach(Customization customization in customizations)
        {
            customization.Randomize();
        }
    }

    public void BackToMenu()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
    }

    public void NextToGame()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 2));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);
        PrefabUtility.SaveAsPrefabAsset(goblin, "Assets/Prefabs/Goblin.prefab");
        SceneManager.LoadScene(levelIndex);
    }
}
