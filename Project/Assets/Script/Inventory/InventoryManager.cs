using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public static InventoryManager MyInstance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<InventoryManager>();
            }

            return instance;
        }
    }

    private List<Bag> bags = new List<Bag>();

    [SerializeField]
    private InventoryButton[] inventoryButtons;

    [SerializeField]
    private Item[] items;

    public bool CanAddBag
    {
        get { return bags.Count < 5; }
    }

    public void Awake()
    {
        Bag bag = (Bag)Instantiate(items[0]);
        bag.Initialize(16);
        bag.Use();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(16);
            bag.Use();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(20);
            AddItem(bag);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Item item = Instantiate(items[1]);
            AddItem(item);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Item item = Instantiate(items[2]);
            AddItem(item);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Item item = Instantiate(items[3]);
            AddItem(item);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Item item = Instantiate(items[4]);
            AddItem(item);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Item item = Instantiate(items[5]);
            AddItem(item);
        }

    }

    public void AddBag(Bag bag)
    {
        foreach(InventoryButton inventoryButton in inventoryButtons)
        {
            if(inventoryButton.MyBag == null)
            {
                inventoryButton.MyBag = bag;
                bags.Add(bag);
                break;
            }
        }
    }

    public void AddItem(Item item)
    {
        if (item.MyStackSize > 0)
        {
            if (PlaceInStack(item))
            {
                return;
            }
        }

        PlaceInEmpty(item);
    }

    private bool PlaceInEmpty(Item item)
    {
        foreach (Bag bag in bags)
        {
            if(bag.MyBagScript.AddItem(item)) {
                return true;
            }
        }

        return false;
    }

    private bool PlaceInStack(Item item)
    {
        foreach(Bag bag in bags)
        {
            foreach (SlotScript slots in bag.MyBagScript.MySlots)
            {
                if (slots.StackItem(item))
                {
                    return true;
                }

            }
        }

        return false;
    }

    public void OpenClose()
    {
        bool closedBag = bags.Find(x => !x.MyBagScript.IsOpen);

        foreach (Bag bag in bags)
        {
            if(bag.MyBagScript.IsOpen != closedBag)
            {
                bag.MyBagScript.OpenClose();
            }
        }
    }
}
