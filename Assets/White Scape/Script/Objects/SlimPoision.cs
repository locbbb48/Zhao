/*    - Codeby Bui Thanh Loc -
    contact : builoc08042004@gmail.com
*/

using UnityEngine;

public class SlimPoision : MonoBehaviour
{
    [SerializeField] private ParticleSystem poisionEffect;
    [SerializeField] private GameObject Slim;
    [SerializeField] private float dmg = 0.5f;
    [SerializeField] private float dur = -1f;
    [SerializeField] private bool isCol = false;

    private void Awake()
    {
        poisionEffect.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == GameManager.Instance.player.gameObject && !isCol)
        {
            UIManager.Instance.ShowNoti("<color=red>You have been poisoned !!!</color>");
            isCol = true;
            poisionEffect.gameObject.SetActive(true);
            Slim.SetActive(false);
            StartCoroutine(GameManager.Instance.player.RemoveHPbyTime(dmg, dur));
        }
    }
}
