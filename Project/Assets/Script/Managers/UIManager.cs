using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    [SerializeField]
    private GameObject tooltip;

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

    private void Awake()
    {
        tooltipText = tooltip.GetComponentInChildren<Text>();
    } 

    void Update()
    {
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
