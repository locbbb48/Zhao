/*	  - Codeby Bui Thanh Loc -
	contact : builoc08042004@gmail.com
*/

using System.Collections;
using UnityEngine;

public abstract class LockBase : MonoBehaviour
{
    protected bool isUnlocked = false;
    private Coroutine unlockCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.Instance.player.gameObject && !isUnlocked)
        {
            UIManager.Instance.ShowLocalizedNoti("LOCK_PRESS_ENTER", 0.5f);
            unlockCoroutine = StartCoroutine(WaitForUnlock());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.Instance.player.gameObject && unlockCoroutine != null)
        {
            StopCoroutine(unlockCoroutine);
            unlockCoroutine = null;
        }
    }

    private IEnumerator WaitForUnlock()
    {
        while (!isUnlocked)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                TryUnlock();
                yield break; // Thoát Coroutine khi đã xử lý mở khóa
            }
            yield return null; // Đợi 1 frame trước khi kiểm tra lại
        }
    }

    protected abstract void TryUnlock();
}
