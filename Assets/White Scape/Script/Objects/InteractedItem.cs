/*	  - Codeby Bui Thanh Loc -
	contact : builoc08042004@gmail.com
*/

using UnityEngine;

public class InteractedItem : MonoBehaviour
{
    [SerializeField] protected bool playerInRange = false;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.Instance.player.gameObject)
        {
            UIManager.Instance.ShowLocalizedNoti("INTERACT_PRESS_ENTER", 0.5f);
            playerInRange = true;
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.Instance.player.gameObject)
        {
            playerInRange = false;
        }
    }
}
