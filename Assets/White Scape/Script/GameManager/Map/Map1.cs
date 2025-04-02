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

        // Gán key cho hai điểm trong danh sách infoPoints
        if (infoPoints.Count >= 2)
        {
            infoPoints[0].messageKey = "MAP1_MOVE_INFO";
            infoPoints[1].messageKey = "MAP1_ENTER_INFO";
        }
        else
        {
            Debug.LogWarning("no InfoPoints.");
        }
    }
}
