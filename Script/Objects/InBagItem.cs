/*	  - Codeby Bui Thanh Loc -
	contact : builoc08042004@gmail.com
*/

using UnityEngine;
using TMPro;

public class InBagItem : MonoBehaviour
{
    /* tạo 1 vật phẩm:
        + tạo prefab, add box col2D
        + gán InBagItem.cs, khia báo tên, ID, số lượng
        + trong Bag -> List -> thêm 1 phần tử
        + gán prefab này vào ô trống
    */
    public PickUpItem pickupItem;
    public float totalQuantity;
    public float maxQuantity;

    public TMP_Text totalQuantity_Text;

    public InBagItem(PickUpItem pickupItem, float maxQuantity)
    {
        this.pickupItem = pickupItem;
        this.maxQuantity = maxQuantity;
        this.totalQuantity = 0;
    }

    private void Update()
    {
        totalQuantity_Text.text = totalQuantity.ToString();
    }
}
