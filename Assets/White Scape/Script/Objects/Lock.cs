/*	  - Codeby Bui Thanh Loc -
	contact : builoc08042004@gmail.com
*/

using UnityEngine;

public class Lock : MonoBehaviour
{
    [SerializeField] private bool isUnlocked = false;
    private bool playerInRange = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isUnlocked)
        {
            UIManager.Instance.ShowNoti("Press Enter to Unlock.");
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && !isUnlocked && Input.GetKeyDown(KeyCode.Return))
        {
            float keyQuantity = Bag.Instance.GetItemQuantity("Key");
            if (keyQuantity > 0)
            {
                Bag.Instance.UseItem(0, 1);
                isUnlocked = true;
                UIManager.Instance.ShowNoti("Unlocked", 1.0f);
                this.gameObject.SetActive(false);
            }
            else
            {
                UIManager.Instance.ShowNoti("You don't have the Key.", 1.0f);
            }
        }
    }
}
