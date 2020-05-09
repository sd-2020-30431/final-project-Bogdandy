using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject[] failResult;

    [SerializeField]
    private Button stealButton, runButton;

    bool check = true;
    bool checkResult = false;

    private static FailWindow instance;

    public static FailWindow MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FailWindow>();
            }

            return instance;
        }
    }

    public void RandomResult()
    {
        ClearResult();
        int rand = Random.Range(0, failResult.Length);

        failResult[rand].SetActive(true);
    }

    public void OpenCloseFailWindow()
    {
        if (UIManager.MyInstance.MyStage <= 11 && checkResult != true)
        {
            UIManager.MyInstance.OpenClose(this.GetComponent<CanvasGroup>());
            check = !check;
            stealButton.interactable = check;
            runButton.interactable = check;

            if (UIManager.MyInstance.MyStage == 11)
            {
                checkResult = true;
            }
        }
        else
        {
            UIManager.MyInstance.StageValue();
        }
    }

    private void ClearResult()
    {
        foreach(GameObject fail in failResult)
        {
            fail.SetActive(false);
        }
    }
}
