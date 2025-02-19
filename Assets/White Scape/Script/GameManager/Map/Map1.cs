/*    - Codeby Bui Thanh Loc -
    contact : builoc08042004@gmail.com
*/
using UnityEngine;

public class Map1 : Map
{
    protected new void Start()
    {
        base.Start();
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is null!");
        }
        else
        {
            GameManager.Instance.isLightOn = true;
        }
    }

    private void Update()
    {
        
    }
}