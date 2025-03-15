/*    - Codeby Bui Thanh Loc -
    contact : builoc08042004@gmail.com
*/
using UnityEngine;
using System.Collections;

public class Map2 : Map
{
    [SerializeField] private float timeToTurnOfftheLight = 10f;
    [SerializeField] private float flashDuration = 1f;
    [SerializeField] private int flashCount = 3;

    private bool TurnOfftheLight = true;
    [SerializeField] private float timeElapsed = 0f;

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
            infoPoints[0].messageKey = "";
            infoPoints[1].messageKey = "";
        }
        else
        {
            Debug.LogWarning("no InfoPoints.");
        }
    }

    protected new void Update()
    {
        if (TurnOfftheLight)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed >= timeToTurnOfftheLight)
            {
                StartCoroutine(FlashLightAndTurnOff());
                TurnOfftheLight = false;
            }
        }
    }

    private IEnumerator FlashLightAndTurnOff()
    {
        for (int i = 0; i < flashCount; i++)
        {
            GameManager.Instance.isLightOn = false; ;  // Turn off the light
            yield return new WaitForSeconds(flashDuration / (flashCount * 2));
            GameManager.Instance.isLightOn = true; ;   // Turn on the light
            yield return new WaitForSeconds(flashDuration / (flashCount * 2));
        }
        GameManager.Instance.isLightOn = false; // Finally turn off the light
        //UIManager.Instance.ShowNoti("New Challenge: Lights Off !!!", 1f);
    }


}
