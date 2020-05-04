using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HandScript : MonoBehaviour
{
    public IMoveable MyMoveable { get; set; }

    private Image icon;

    [SerializeField]
    public Vector3 offset;

    private static HandScript instance;
    
    public static HandScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HandScript>();
            }

            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponent<Image>();   
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos;
        mousePos = Input.mousePosition;
        icon.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        this.transform.localPosition = new Vector3(mousePos.x + offset.x, mousePos.y + offset.y, 0) ;

        DeleteItem();
    }

    public void TakeMoveable(IMoveable moveable)
    {
        this.MyMoveable = moveable;
        icon.sprite = moveable.MyIcon;
        icon.color = Color.white;
    }

    public IMoveable Put()
    {
        IMoveable aux = MyMoveable;
        MyMoveable = null;
        icon.color = new Color(0, 0, 0, 0);
        return aux;
    }

    public void Drop()
    {
        MyMoveable = null;
        icon.color = new Color(0, 0, 0, 0);
    }

    private void DeleteItem()
    {
        if(Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && MyInstance.MyMoveable!= null)
        {
            if(MyMoveable is Item && InventoryManager.MyInstance.FromSlot != null)
            {
                (MyMoveable as Item).MySlot.Clear();
            }

            Drop();

            InventoryManager.MyInstance.FromSlot = null;
        }
    }
}
