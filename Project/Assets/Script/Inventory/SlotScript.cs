﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    private ObservableStack<Item> items = new ObservableStack<Item>();

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text stackSize;

    public BagScript MyBag { get; set; }

    public bool IsEmpty
    {
        get
        {
            return MyItems.Count == 0;
        }
    }

    public bool IsFull
    {
        get
        {
            if (IsEmpty || MyCount < MyItem.MyStackSize)
            {
                return false;
            }

            return true;
        }
    }

    public Item MyItem
    {
        get
        {
            if(!IsEmpty){
                return MyItems.Peek();
            }

            return null;
        }
    }

    public Image MyIcon
    {
        get
        {
            return icon;
        }

        set
        {
            icon = value;
        }
    }

    public Text MyStackText
    {
        get
        {
            return stackSize;
        }
    }
    
    public void Awake()
    {
        MyItems.OnPop += new UpdateStackEvent(UpdateSlot);
        MyItems.OnPush += new UpdateStackEvent(UpdateSlot);
        MyItems.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    public int MyCount
    {
        get
        {
            return MyItems.Count;
        }
    }

    public ObservableStack<Item> MyItems
    {
        get
        {
            return items;
        }
    }

    public bool AddItem(Item item)
    {
        MyItems.Push(item);
        icon.sprite = item.MyIcon;
        icon.color = Color.white;
        item.MySlot = this;

        return true;
    }

    public bool SwapItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }

        int x = from.MyCount + MyCount;

        if ( from.MyItem.GetType() != MyItem.GetType() || x > MyItem.MyStackSize )
        {
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.MyItems);

            from.MyItems.Clear();
            from.AddItems(MyItems);

            MyItems.Clear();
            AddItems(tmpFrom);

            return true;
        }

        return false;
    }

    private bool MergeItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }

        if (from.MyItem.GetType() == MyItem.GetType() && !IsFull)
        {
            int free = MyItem.MyStackSize - MyCount;

            for(int i = 0; i < free; i++)
            {
                if (from.MyCount > 0)
                {
                    AddItem(from.MyItems.Pop());
                }
            }

            return true;
        }

        return false;
    }

    public bool AddItems(ObservableStack<Item> newItems)
    {
        if (IsEmpty || newItems.Peek().GetType() == MyItem.GetType())
        {
            int count = newItems.Count;
            for (int i = 0; i < count; i++)
            {
                if (IsFull)
                {
                    return false;
                }

                AddItem(newItems.Pop());
            }

            return true;
        }

        return false;
    }

    public void RemoveItem(Item item)
    {
        if(!IsEmpty)
        {
            MyItems.Pop();
        }
    }

    public void Clear()
    {
        if(MyItems.Count > 0)
        {
            MyItems.Clear();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryManager.MyInstance.FromSlot == null && !IsEmpty) {

                if (HandScript.MyInstance.MyMoveable != null && (HandScript.MyInstance.MyMoveable is Bag))
                {
                    if(MyItem is Bag)
                    {
                        InventoryManager.MyInstance.SwapBags(HandScript.MyInstance.MyMoveable as Bag, MyItem as Bag);
                    }
                }
                else
                {
                    HandScript.MyInstance.TakeMoveable(MyItem as IMoveable);
                    InventoryManager.MyInstance.FromSlot = this;
                }
                 
            }
            else if (InventoryManager.MyInstance.FromSlot == null && IsEmpty && (HandScript.MyInstance.MyMoveable is Bag))
            {
                Bag bag = (Bag)HandScript.MyInstance.MyMoveable;

                if (bag.MyBagScript != MyBag && (InventoryManager.MyInstance.MyEmptySlotCount - bag.MySlots > 0))
                {
                    AddItem(bag);
                    bag.MyInventoryButton.RemoveBag();
                    HandScript.MyInstance.Drop();
                }
            }
            else if (InventoryManager.MyInstance.FromSlot != null)
            {
                if (PutItemBack() || MergeItems(InventoryManager.MyInstance.FromSlot) ||  SwapItems(InventoryManager.MyInstance.FromSlot) || AddItems(InventoryManager.MyInstance.FromSlot.items) )
                {
                    HandScript.MyInstance.Drop();
                    InventoryManager.MyInstance.FromSlot = null;
                }
            }
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsEmpty)
        {
            UIManager.MyInstance.ShowTooltip(transform.position, MyItem);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideTooltip();
    }

    public void UseItem()
    {
        if(MyItem is IUseable)
        {
            (MyItem as IUseable).Use();
        }
    }

    public bool StackItem(Item item)
    {
        if (!IsEmpty && item.name == MyItem.name && MyItems.Count < MyItem.MyStackSize)
        {
            MyItems.Push(item);
            item.MySlot = this;
            return true;
        }

        return false;

    }

    public bool PutItemBack()
    {
        if(InventoryManager.MyInstance.FromSlot == this)
        {
            InventoryManager.MyInstance.FromSlot.MyIcon.color = Color.white;
            return true;
        }

        return false;
    }

    private void UpdateSlot()
    {
        UIManager.MyInstance.UpdateStackSize(this);
    }
}
