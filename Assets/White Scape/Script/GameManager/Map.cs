/*    - Codeby Bui Thanh Loc -
    contact : builoc08042004@gmail.com
*/

using TMPro;
using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class InfoPoint
{
    public Transform point;
    public Sprite sprite;
    public string messageKey;
    public bool shown = false;
}

public class Map : MonoBehaviour
{
    public GameObject Prefab;
    public GameObject startPoint;
    public EndPoint endPoint;

    [SerializeField] protected List<InfoPoint> infoPoints;

    protected void Start()
    {
        Prefab = GetComponent<GameObject>();
    }
    protected void OnEnable()
    {
        MovePlayerToStartPoint();
    }

    protected virtual void Update()
    {
        CheckInfoPoints();
    }

    private void CheckInfoPoints()
    {
        if (GameManager.Instance?.player == null) return;

        foreach (var info in infoPoints)
        {
            if (!info.shown && info.point != null)
            {
                float distance = Vector3.Distance(GameManager.Instance.player.transform.position, info.point.position);
                if (distance <= 1)
                {
                    ShowInfo(info);
                }
            }
        }
    }

    private void ShowInfo(InfoPoint info)
    {
        UIManager.Instance.ShowInforPanel(info.messageKey, info.sprite);
        info.shown = true;
    }

    protected void MovePlayerToStartPoint()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && startPoint != null)
        {
            player.transform.position = startPoint.transform.position;
        }
    }

    public int PickUpItemLeft()
    {
        PickUpItem[] items = GetComponentsInChildren<PickUpItem>();
        int count = 0;

        foreach (var item in items)
        {
            if (!item.isPickUped)
            {
                count++;
            }
        }

        return count;
    }

    public int InteractedItemLeft()
    {
        InteractedItem[] items = GetComponentsInChildren<InteractedItem>();
        int count = 0;

        foreach (var item in items)
        {
            if (item != null)
            {
                count++;
            }
        }
        return count;
    }
}
