/*    - Codeby Bui Thanh Loc -
    contact : builoc08042004@gmail.com
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractedItem
{
    [SerializeField] private List<PickUpItem> items;

    private void Awake()
    {
        foreach (var item in items)
        {
            item.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Return))
        {
            UIManager.Instance.ShowNoti("Chest opened!");

            foreach (var item in items)
            {
                item.transform.SetParent(null);
                item.gameObject.SetActive(true);
            }

            StartCoroutine(HideChestAfterDelay(0.5f));
        }
    }

    private IEnumerator HideChestAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        this.gameObject.SetActive(false);
    }
}
