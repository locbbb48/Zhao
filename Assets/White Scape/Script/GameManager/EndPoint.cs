/*    - Codeby Bui Thanh Loc -
    contact : builoc08042004@gmail.com
*/

using UnityEngine;

public class EndPoint : MonoBehaviour
{
    [SerializeField] private bool isCol = false;
    private BoxCollider2D box;
    private void Awake()
    {
        box = GetComponent<BoxCollider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == GameManager.Instance.player.gameObject && !isCol)
        {
            isCol = true;
            UIManager.Instance.nextMapPanel.gameObject.SetActive(true);

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == GameManager.Instance.player.gameObject)
        {
            isCol = false;
        }
    }
}
