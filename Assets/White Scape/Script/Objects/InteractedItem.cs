/*    - Codeby Bui Thanh Loc -  
    contact : builoc08042004@gmail.com  
*/

using UnityEngine;

public class InteractedItem : MonoBehaviour
{
    private void Start()
    {
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider2D>();
        }
        collider.isTrigger = true;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.Instance.player.gameObject)
        {
            UIManager.Instance.ShowLocalizedNoti("INTERACT_PRESS_ENTER", 0.5f);
        }
    }
}
