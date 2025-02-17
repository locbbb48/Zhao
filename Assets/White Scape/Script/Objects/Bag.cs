/*	  - Codeby Bui Thanh Loc -
	contact : builoc08042004@gmail.com
*/

using System.Collections.Generic;
using UnityEngine;

/*  Name    |   ID
    Key         0
    Coin        1
    Book        2
*/
public class Bag : MonoBehaviour
{
    public static Bag Instance { get; private set; }

    public List<InBagItem> inBagItems = new List<InBagItem>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void AddItem(PickUpItem puI)
    {
        InBagItem inBagItem = inBagItems.Find(item => item.pickupItem.ID == puI.ID);

        if (inBagItem != null)
        {
            if (inBagItem.totalQuantity + puI.quantity <= inBagItem.maxQuantity)
            {
                inBagItem.totalQuantity += puI.quantity;
                puI.isPickUped = true;
                UIManager.Instance.ShowLocalizedNoti(
                    "BAG_ITEM_ADDED", 1f, puI.quantity, puI.Name, inBagItem.totalQuantity
            );
            }
            else
            {
                UIManager.Instance.ShowLocalizedNoti(
                    "BAG_ITEM_EXCEED", 1f, puI.quantity, puI.Name
            );
            }
        }
        else
        {
            inBagItem = new InBagItem(puI, 10000);
            inBagItem.totalQuantity = puI.quantity;
            inBagItems.Add(inBagItem);
            puI.isPickUped = true;
            UIManager.Instance.ShowLocalizedNoti(
                "BAG_ITEM_ADDED", 1f, puI.quantity, puI.Name, inBagItem.totalQuantity
            );
        }
    }

    public void UseItem(int itemId, float quantity)
    {
        InBagItem inBagItem = inBagItems.Find(item => item.pickupItem.ID == itemId);

        if (inBagItem != null)
        {
            string itemName = inBagItem.pickupItem.Name;

            if (inBagItem.totalQuantity >= quantity)
            {
                inBagItem.totalQuantity -= quantity;

                UIManager.Instance.ShowLocalizedNoti(
                    "BAG_ITEM_USED", 1f, quantity, itemName, inBagItem.totalQuantity
                );

                if (inBagItem.totalQuantity <= 0)
                {
                    UIManager.Instance.ShowLocalizedNoti(
                        "BAG_ITEM_EMPTY", 1f, itemName
                    );
                }
            }
            else
            {
                UIManager.Instance.ShowLocalizedNoti(
                "BAG_ITEM_NOT_ENOUGH", 1f, itemName
                );
            }
        }
        else
        {
            UIManager.Instance.ShowLocalizedNoti(
            "BAG_ITEM_NOT_FOUND_ID", 1f, itemId
            );
        }
    }



    public float GetItemQuantity(string itemName)
    {
        InBagItem inBagItem = inBagItems.Find(item => item.pickupItem.Name == itemName);

        if (inBagItem != null)
        {
            return inBagItem.totalQuantity;
        }
        else
        {
            UIManager.Instance.ShowLocalizedNoti(
            "BAG_ITEM_NOT_FOUND_NAME", 1f, itemName
            );
            return 0;
        }
    }
}
