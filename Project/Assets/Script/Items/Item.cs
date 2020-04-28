using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject, IDescribable
{
    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private int stackSize;

    [SerializeField]
    private int price;

    [SerializeField]
    private string description;

    private SlotScript slot;

    public Sprite MyIcon
    {
        get
        {
            return icon;
        }
    }

    public int MyStackSize
    {
        get
        {
            return stackSize;
        }
    }

    public SlotScript MySlot
    {
        get
        {
            return slot;
        }

        set
        {
            slot = value;
        }
    }

    public void Remove()
    {
        if(MySlot != null)
        {
            MySlot.RemoveItem(this);
        }
    }

    public virtual string GetDescription()
    {
        if (stackSize == 0)
        {
            return "\nPRICE " + price + "\n" + description.ToUpper();
        }
        else
        {
            return this.name.Replace("(Clone)", "").ToUpper() + "\nSTACK SIZE  " + stackSize + "\nPRICE  " + price + "\n" + description.ToUpper();
        }

    }
}
