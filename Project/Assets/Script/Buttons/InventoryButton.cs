using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Bag bag;

    [SerializeField]
    private Sprite full, empty;

    [SerializeField]
    private int bagIndex;

    public Bag MyBag
    {
        get
        {
            return bag;
        }

        set
        {
            if (value != null)
            {
                GetComponent<Image>().sprite = full;
            }
            else
            {
                GetComponent<Image>().sprite = empty;
            }

            bag = value;
        }
    }

    public int MyBagIndex
    {
        get
        {
            return bagIndex;
        }

        set
        {
            bagIndex = value;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryManager.MyInstance.FromSlot != null && HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is Bag)
            {
                if(MyBag != null)
                {
                    InventoryManager.MyInstance.SwapBags(MyBag, HandScript.MyInstance.MyMoveable as Bag);
                }
                else
                {
                    Bag aux = (Bag)HandScript.MyInstance.MyMoveable;
                    aux.MyInventoryButton = this;
                    aux.Use();
                    MyBag = aux;
                    HandScript.MyInstance.Drop();
                    InventoryManager.MyInstance.FromSlot = null;
                }
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                HandScript.MyInstance.TakeMoveable(MyBag);
            }
            else if (bag != null)
            {
                bag.MyBagScript.OpenClose();
            }
        }

       
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (bag != null)
        {
            //UIManager.MyInstance.ShowTooltip(transform.position);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       // UIManager.MyInstance.HideTooltip();
    }

    public void RemoveBag()
    {
        InventoryManager.MyInstance.RemoveBag(MyBag);
        MyBag.MyInventoryButton = null;

        foreach (Item item in MyBag.MyBagScript.GetItems())
        {
            InventoryManager.MyInstance.AddItem(item);
        }

        MyBag = null;
    }
}
