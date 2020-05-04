using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Bag", menuName = "Items/Bag", order = 1)]
public class Bag : Item, IUseable
{
    private int slots;

    [SerializeField]
    private GameObject bagPrefab;

    public BagScript MyBagScript{ get; set; }

    public InventoryButton MyInventoryButton { get; set; }

    public int MySlots
    {
        get
        {
            return slots;
        }
    }

    public void Initialize(int slots)
    {
        this.slots = slots;
    }

    public void Use()
    {
        if (InventoryManager.MyInstance.CanAddBag)
        {
            Remove();
            MyBagScript = Instantiate(bagPrefab, InventoryManager.MyInstance.transform).GetComponent<BagScript>();
            MyBagScript.AddSlots(slots);

            if (MyInventoryButton == null)
            {
                InventoryManager.MyInstance.AddBag(this);
            }
            else
            {
                InventoryManager.MyInstance.AddBag(this, MyInventoryButton);
            }
       
        }


    }

    public override string GetDescription()
    {
        return "BAG\nCAPACITY  "+ slots + base.GetDescription();
    }
}
