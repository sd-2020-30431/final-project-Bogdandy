using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LootButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text title;

    private LootWindow lootWindow;

    public Image MyIcon
    {
        get
        {
            return icon;
        }
    }

    public Text MyTitle
    {
        get
        {
            return title;
        }
    }

    private void Awake()
    {
        lootWindow = GetComponentInParent<LootWindow>();
    }

    public Item MyLoot { get; set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (InventoryManager.MyInstance.AddItem(Instantiate(MyLoot)))
        {
            gameObject.SetActive(false);
            lootWindow.RetrieveLoot(MyLoot);
            UIManager.MyInstance.HideTooltip();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.MyInstance.ShowTooltip(transform.position, MyLoot);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideTooltip();
    }

}
