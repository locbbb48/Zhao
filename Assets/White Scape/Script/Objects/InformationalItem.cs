/*	  - Codeby Bui Thanh Loc -
	contact : builoc08042004@gmail.com
*/

using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class InformationalItem : InteractedItem
{
    private Light2D itemLight;

    [SerializeField] private float defaultLightIntensity = 0.01f;
    [SerializeField] private float activeLightIntensity = 5f;
    [SerializeField] private float defaultLightRadius = 0.5f;
    [SerializeField] private Color defaultLightColor = new Color(1f, 0.85f, 0.3f);


    [SerializeField] private Sprite itemImage;
    [SerializeField] private string itemText;

    private void Start()
    {
        itemLight = gameObject.AddComponent<Light2D>();
        itemLight.lightType = Light2D.LightType.Point;
        itemLight.pointLightOuterRadius = defaultLightRadius; // Bán kính ánh sáng
        itemLight.intensity = defaultLightIntensity; // Độ sáng ban đầu
        itemLight.color = defaultLightColor; // Đặt màu 
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        itemLight.intensity = activeLightIntensity; // Tăng sáng
        StartCoroutine(WaitForInteract()); // Chờ nhấn Enter
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        itemLight.intensity = defaultLightIntensity; // Giảm sáng
        StopAllCoroutines(); // Dừng chờ nhấn Enter
    }

    private IEnumerator WaitForInteract()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                UIManager.Instance.ShowInforPanel(itemText, itemImage);
                yield break; // Thoát khỏi vòng lặp khi nhấn Enter
            }
            yield return null; // Đợi frame tiếp theo
        }
    }
}
