/*    - Codeby Bui Thanh Loc -  
    contact : builoc08042004@gmail.com  
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractedItem
{
    [SerializeField] private List<PickUpItem> items;
    private bool isOpened = false;

    private void Awake()
    {
        foreach (var item in items)
        {
            item.gameObject.SetActive(false);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.gameObject == GameManager.Instance.player.gameObject && !isOpened)
        {
            StartCoroutine(HandleChestOpening());
        }
    }

    private IEnumerator HandleChestOpening()
    {
        isOpened = true;
        UIManager.Instance.ShowLocalizedNoti("CHEST_OPENED", 0.5f);

        foreach (var item in items)
        {
            item.transform.SetParent(null);
            item.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }
}
