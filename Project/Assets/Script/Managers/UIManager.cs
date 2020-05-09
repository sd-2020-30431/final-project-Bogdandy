using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    private int score = 0;
    private int stage = 1;

    [SerializeField]
    private GameObject[] knights;

    [SerializeField]
    private GameObject[] scoreTextResults;

    [SerializeField]
    private GameObject tooltip;

    [SerializeField]
    private Text knightNoText;

    [SerializeField]
    private Text scoreText;
    
    [SerializeField]
    private Text stageText;

    [SerializeField]
    private Text endWindowScore;

    [SerializeField]
    private GameObject resultWindow;

    private Text tooltipText;

    public static UIManager MyInstance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }

    public int MyStage
    {
        get
        {
            return stage;
        }

        set
        {
            stage = value;
        }
    }

    public GameObject MyResultWindow
    {
        get
        {
            return resultWindow;
        }

        set
        {
            resultWindow = value;
        }
    }

    private void FindNoKnights(){
        int knightNo = 0;

        foreach(GameObject knight in knights)
        {
            if (knight.GetComponent<SpriteRenderer>().sprite != null)
            {
                knightNo++;
            }
        }

        knightNoText.text = "KNIGHTS:  " + knightNo;
    }

    public void StageValue()
    {
        stage++;
        Debug.Log(stage);
        if (stage <= 11)
        {
            if(stage < 11)
            {
                stageText.text = "STAGE: " + stage;
            }
        }
        else
        {
            if (score <= 1500)
            {
                scoreTextResults[0].SetActive(true);
            }
            else if (score <= 3000)
            {
                scoreTextResults[1].SetActive(true);
            }
            else
            {
                scoreTextResults[2].SetActive(true);
            }
            endWindowScore.text = "SCORE:  " + score;
            resultWindow.SetActive(true);
        }
    }

    private void ComputeScore()
    {
        score = 0;
        List<Bag> bags = InventoryManager.MyInstance.MyBags;

        foreach (Bag bag in bags)
        {
            List<SlotScript> slotScripts = bag.MyBagScript.MySlots;

            foreach (SlotScript slotScript in slotScripts)
            {
                int scoreItem;
                int.TryParse(slotScript.MyStackText.text, out scoreItem);
                if(slotScript.MyItem != null)
                {
                    if (scoreItem == 0) {
                        score += slotScript.MyItem.MyPrice;
                    }
                    else
                    {
                        score += scoreItem * slotScript.MyItem.MyPrice;
                    }
                }
            }
        }

        scoreText.text = "SCORE:  " + score;
    }

    private void Awake()
    {
        tooltipText = tooltip.GetComponentInChildren<Text>();
    } 

    void Update()
    {
        ComputeScore();
        FindNoKnights();

        if (Input.GetKeyDown(KeyCode.B))
        {
            InventoryManager.MyInstance.OpenClose();
        }
    }

    public void OpenClose(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;

        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public void UpdateStackSize(IClickable clickable)
    {
        if (clickable.MyCount > 1)
        {
            clickable.MyStackText.text = clickable.MyCount.ToString();
            clickable.MyStackText.color = Color.white;
            clickable.MyIcon.color = Color.white;
        }
        else
        {
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
            clickable.MyIcon.color = Color.white;
        }

        if (clickable.MyCount == 0)
        {
            clickable.MyIcon.color = new Color(0, 0, 0, 0);
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
        }
    }

    public void ShowTooltip(Vector3 position, IDescribable description)
    {
        tooltip.SetActive(true);
        tooltip.transform.position = position;
        tooltipText.text = description.GetDescription();
    }

    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }
}
