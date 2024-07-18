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
                UIManager.Instance.ShowNoti($"Added {puI.quantity} {puI.Name}(s) to the bag. Total: {inBagItem.totalQuantity}");
            }
            else
            {
                UIManager.Instance.ShowNoti($"Cannot add {puI.quantity} {puI.Name}(s) to the bag. Exceeds maximum quantity.");
            }
        }
        else
        {
            inBagItem = new InBagItem(puI, 10000);
            inBagItem.totalQuantity = puI.quantity;
            inBagItems.Add(inBagItem);
            puI.isPickUped = true;
            UIManager.Instance.ShowNoti($"Added {puI.quantity} {puI.Name}(s) to the bag. Total: {inBagItem.totalQuantity}");
        }
    }

    public void UseItem(int itemId, float quantity)
    {
        InBagItem inBagItem = inBagItems.Find(item => item.pickupItem.ID == itemId);

        if (inBagItem != null)
        {
            if (inBagItem.totalQuantity >= quantity)
            {
                inBagItem.totalQuantity -= quantity;

                UIManager.Instance.ShowNoti($"Used {quantity} of item ID {itemId}. Remaining: {inBagItem.totalQuantity}");

                if (inBagItem.totalQuantity <= 0)
                {
                    inBagItems.Remove(inBagItem);
                    UIManager.Instance.ShowNoti($"Item ID {itemId} is completely used up and removed from the bag.");
                }
            }
            else
            {
                UIManager.Instance.ShowNoti($"Not enough items with ID {itemId} to use.");
            }
        }
        else
        {
            UIManager.Instance.ShowNoti($"Item with ID {itemId} not found in the bag.");
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
            UIManager.Instance.ShowNoti($"Item with name {itemName} not found in the bag.");
            return 0;
        }
    }
}
