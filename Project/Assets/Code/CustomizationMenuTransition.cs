using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomizationMenuTransition : MonoBehaviour
{
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
