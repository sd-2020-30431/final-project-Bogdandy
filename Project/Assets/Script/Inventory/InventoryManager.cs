using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ItemCountChanged(Item item);

public class InventoryManager : MonoBehaviour
{
    public event ItemCountChanged itemCountChangedEvent;

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

    private SlotScript fromSlot;

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
        bag.Initialize(12);
        bag.Use();
        bag.MyBagScript.OpenClose();
    }
    
    public SlotScript FromSlot
    {
        get
        {
            return fromSlot;
        }

        set
        {
            fromSlot = value;

            if(value != null)
            {
                fromSlot.MyIcon.color = Color.grey;
            }

            fromSlot = value;
        }
    }

    /*private void Update()
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

    }*/

    public void OnItemCountChanged(Item item)
    {
        if(itemCountChangedEvent != null)
        {
            itemCountChangedEvent.Invoke(item);
        }
    }

    public int MyEmptySlotCount
    {
        get
        {
            int count = 0;

            foreach (Bag bag in bags)
            {
                count+=bag.MyBagScript.MyEmptySlotCount;
            }

            return count;
        }
    }

    public int MyFullSlotCount
    {
        get
        {
            return MyTotalSlotCount - MyEmptySlotCount;
        }
    }

    public int MyTotalSlotCount
    {
        get
        {
            int count = 0;

            foreach(Bag bag in bags)
            {
                count += bag.MyBagScript.MySlots.Count;
            }

            return count;
        }
    }

    public List<Bag> MyBags
    {
        get
        {
            return bags;
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
                bag.MyInventoryButton = inventoryButton;
                bag.MyBagScript.transform.SetSiblingIndex(inventoryButton.MyBagIndex);
                break;
            }
        }
    }

    public void AddBag(Bag bag, InventoryButton inventoryButton)
    {
        bags.Add(bag);
        inventoryButton.MyBag = bag;
        bag.MyBagScript.transform.SetSiblingIndex(inventoryButton.MyBagIndex);
    }

    public void SwapBags(Bag oldBag, Bag newBag)
    {
        int newSlotCount= (MyTotalSlotCount - oldBag.MySlots) + newBag.MySlots;

        if (newSlotCount - MyFullSlotCount >= 0)
        {
            List<Item> bagItems = oldBag.MyBagScript.GetItems();

            RemoveBag(oldBag);

            newBag.MyInventoryButton = oldBag.MyInventoryButton;

            newBag.Use();

            foreach(Item item in bagItems)
            {
                if (item != newBag)
                {
                    AddItem(item);
                }
            }

            AddItem(oldBag);

            HandScript.MyInstance.Drop();

            MyInstance.fromSlot = null;
        }
    }

    public void RemoveBag(Bag bag)
    {
        bags.Remove(bag);
        Destroy(bag.MyBagScript.gameObject);
    }

    public bool AddItem(Item item)
    {
        if (item.MyStackSize > 0)
        {
            if (PlaceInStack(item))
            {
                return true;
            }
        }

        return PlaceInEmpty(item);
    }

    private bool PlaceInEmpty(Item item)
    {
        foreach (Bag bag in bags)
        {
            if(bag.MyBagScript.AddItem(item)) {
                OnItemCountChanged(item);
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
                    OnItemCountChanged(item);
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
