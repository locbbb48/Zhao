/*    - Codeby Bui Thanh Loc -
    contact : builoc08042004@gmail.com
*/

using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject Prefab;
    public GameObject startPoint;
    public EndPoint endPoint;

    protected void Start()
    {
        Prefab = GetComponent<GameObject>();
    }
    protected void OnEnable()
    {
        MovePlayerToStartPoint();
    }

    protected void MovePlayerToStartPoint()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && startPoint != null)
        {
            player.transform.position = startPoint.transform.position;
        }
    }
}
