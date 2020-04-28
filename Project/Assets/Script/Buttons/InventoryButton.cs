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
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (bag != null)
        {
            bag.MyBagScript.OpenClose();
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
        //UIManager.MyInstance.HideTooltip();
    }

}
