/*	  - Codeby Bui Thanh Loc -
	contact : builoc08042004@gmail.com
*/

using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public string Name = "unKnown";
    public int ID = -1;
    public float quantity = 0;
    public bool isPickUped = false;

    public PickUpItem(string name, int iD, float quantity, bool isPickUped)
    {
        Name = name;
        ID = iD;
        this.quantity = quantity;
        this.isPickUped = isPickUped;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Bag.Instance.AddItem(this);
            this.gameObject.SetActive(false);
        }
    }
}
