/*    - Codeby Bui Thanh Loc -
    contact : builoc08042004@gmail.com
*/

using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject Prefab;
    public GameObject startPoint;
    public EndPoint endPoint;

    private void Start()
    {
        Prefab = GetComponent<GameObject>();
    }
    private void OnEnable()
    {
        MovePlayerToStartPoint();
    }

    private void MovePlayerToStartPoint()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && startPoint != null)
        {
            player.transform.position = startPoint.transform.position;
        }
    }
}
