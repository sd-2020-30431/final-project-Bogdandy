using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootWindow : MonoBehaviour
{
    [SerializeField]
    private LootButton[] lootButtons;
    
    private List<List<Item>> pages = new List<List<Item>>();
    private int[] bagSizes = { 4, 8, 12, 16, 20 };
    private int pageIndex = 0;

    [SerializeField]
    private Text pageNumber;

    [SerializeField]
    private GameObject nextButton, previousButton;

    [SerializeField]
    private Button stealButton, runButton;

    [SerializeField]
    private Item[] items;

    private int[] chances = { 3, 7, 10, 15, 25, 40 };
    private int total;
    private int randomNo;
    List<Item> droppedLoot;

    bool check = true;
    bool checkResult = false;

    private static LootWindow instance;

    public static LootWindow MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LootWindow>();
            }

            return instance;
        }
    }
    public void Randomize()
    {
        droppedLoot = new List<Item>();

        randomNo = Random.Range(0, 100);

        for (int i = 0; i < items.Length; i++)
        {
            if (randomNo <= chances[i])
            {
                droppedLoot.Add(items[i]);
            }
            else
            {
                randomNo -= chances[i];
            }
        }

        pageIndex = 0;
        pageNumber.text = pageIndex + 1 + "/" + 1;
        pages = new List<List<Item>>();
        CreatePages(droppedLoot);
    }

    public void OpenCloseLootWindow()
    {
        if (UIManager.MyInstance.MyStage <= 11 && checkResult != true)
        {
            UIManager.MyInstance.OpenClose(this.GetComponent<CanvasGroup>());
            check = !check;
            stealButton.interactable = check;
            runButton.interactable = check;

            if(UIManager.MyInstance.MyStage == 11)
            {
                checkResult = true;
            }
        }
        else
        {
            UIManager.MyInstance.StageValue();
        }

    }

    public void Update()
    {
        if (UIManager.MyInstance.MyStage > 10)
        {
            stealButton.interactable = false;
            runButton.interactable = false;
        }
    }

    public void CreatePages(List<Item> droppedLoot)
    {
        List<Item> page = new List<Item>();
        int length = droppedLoot.Count - 1;

        for(int i = 0; i <= length; i++)
        {
            page.Add(droppedLoot[i]);
            if(page.Count == 4 || i == length )
            {
                pages.Add(page);
                page = new List<Item>();
            }
        }

        AddLoot();
    }

    private void AddLoot()
    {
        if(pages.Count > 0)
        {
            pageNumber.text = pageIndex + 1 + "/"+pages.Count;

            previousButton.SetActive(pageIndex > 0);
            nextButton.SetActive(pages.Count > 1 && (pageIndex < pages.Count - 1));

            for (int i = 0; i < pages[pageIndex].Count; i++) 
            {
                if (pages[pageIndex][i] != null)
                {
                    lootButtons[i].MyIcon.sprite = pages[pageIndex][i].MyIcon;

                    if (pages[pageIndex][i] is Bag)
                    {
                        Bag bag = (Bag)pages[pageIndex][i];

                        int rand = Random.Range(0, bagSizes.Length - 1);
                        bag.Initialize(bagSizes[rand]);
                        lootButtons[i].MyLoot = bag;
                    }
                    else
                    {
                        lootButtons[i].MyLoot = pages[pageIndex][i];
                    }

                    lootButtons[i].gameObject.SetActive(true);
                    lootButtons[i].MyTitle.text = pages[pageIndex][i].MyTitle;
                }
                
            }
        }

    }

    public void ClearButtons()
    {
        foreach(LootButton button in lootButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void NextPage()
    {
        if (pageIndex < pages.Count - 1)
        {
            pageIndex++;
            ClearButtons();
            AddLoot();
        }
    }

    public void PreviousPage()
    {
        if (pageIndex > 0)
        {
            pageIndex--;
            ClearButtons();
            AddLoot();
        }
    }

    public void RetrieveLoot(Item loot)
    {
        pages[pageIndex].Remove(loot);

        if (pages[pageIndex].Count == 0)
        {
            pages.Remove(pages[pageIndex]);

            if (pageIndex == pages.Count && pageIndex > 0)
            {
                pageIndex--;
            }

            AddLoot();
        }
    }
}
