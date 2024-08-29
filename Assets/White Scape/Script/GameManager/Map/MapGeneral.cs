/*    - Codeby Bui Thanh Loc -
    contact : builoc08042004@gmail.com
*/
using UnityEngine;

public class MapGeneral : Map
{

    [SerializeField] bool isLight = false;
    protected new void Start()
    {
        base.Start();
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is null!");
        }
        else
        {
            GameManager.Instance.isLightOn = isLight;
        }
    }
}