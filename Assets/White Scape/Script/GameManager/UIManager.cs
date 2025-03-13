/*    - Codeby Bui Thanh Loc -
    contact : builoc08042004@gmail.com
*/

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    /* tạo 1 panel:
        + tạo panel
        + trong EventSystem -> UIManager -> List -> +
        + gán panel này vào ô trống
    */
    public static UIManager Instance { get; private set; }

    [SerializeField] private Player player;

    private Queue<string> notificationQueue = new Queue<string>();
    private List<string> notificationLog = new List<string>(); // Danh sách lưu trữ thông báo
    private bool isShowingNotification = false;
    public TMP_Text debugText;
    public GameObject logPanel;   // Panel hiển thị lịch sử thông báo
    public TMP_Text logText;

    [SerializeField] private GameObject inforPanel; // Bảng hiển thị thông tin liên quán đến màn chơi
    public TMP_Text inforText;
    public Image inforImage;
    public Button inforButton;

    public GameObject bagIcon;
    public GameObject bagPanel;

    public GameObject shopPanel;
    public GameObject buyPanel;
    private float currentCost;
    [SerializeField] private GameObject currentBuyItemButton;

    public GameObject nextMapPanel;
    public Button NoButton;
    public Button YesButton;
    public TMP_Text pickupItemLeft;


    [SerializeField] private List<GameObject> panels;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        YesButton.onClick.AddListener(() => HandleNextMap(true));
        NoButton.onClick.AddListener(() => HandleNextMap(false));
    }

    public void ShowDamage(float damageAmount, Vector3 position, Color color)
    {
        // Lấy một instance của DamageText từ Object Pool
        TextMesh damageText = ShowDameTextPool.Instance.GetObject();
        damageText.transform.position = position;
        damageText.text = damageAmount.ToString();
        damageText.color = color;
        damageText.gameObject.SetActive(true);

        // Bắt đầu Coroutine để xử lý hiệu ứng
        StartCoroutine(MoveAndFade(damageText));
    }

    private IEnumerator MoveAndFade(TextMesh damageText)
    {
        float displayDuration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < displayDuration)
        {
            // Di chuyển lên trên và làm mờ
            damageText.transform.position += new Vector3(0, Time.deltaTime, 0);
            Color color = damageText.color;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / displayDuration);
            damageText.color = color;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Trả lại đối tượng về Object Pool
        ShowDameTextPool.Instance.ReturnObject(damageText);
    }

    // Show Raw Noti
    public void ShowNoti(string message, float duration = 0.5f)
    {
        notificationQueue.Enqueue(message);
        notificationLog.Add(message);
        logText.text += message + "\n";
        if (!isShowingNotification)
        {
            StartCoroutine(ShowNotification(duration));
        }
    }

    public void ShowInforPanel(string localizationKey, Sprite image = null)
    {
        EventSystem.current.SetSelectedGameObject(inforButton.gameObject); // Xóa button được chọn
        string localizedMessage = LocalizationManager.instance.GetLocalizedValue(localizationKey);
        inforText.text = localizedMessage;

        if (image != null)
        {
            inforImage.sprite = image;
            inforImage.gameObject.SetActive(true);
        }
        else
        {
            inforImage.gameObject.SetActive(false);
        }

        inforPanel.SetActive(true);
    }

    private IEnumerator ShowNotification(float duration)
    {
        while (notificationQueue.Count > 0)
        {
            isShowingNotification = true;
            string message = notificationQueue.Dequeue();
            debugText.text = message;
            debugText.gameObject.SetActive(true);

            yield return new WaitForSeconds(duration);

            debugText.text = string.Empty;
            debugText.gameObject.SetActive(false);
        }
        isShowingNotification = false;
    }

    public void ShowNotificationLog()
    {
        logPanel.SetActive(true); // Hiển thị panel chứa log
    }


    // Hiển thị thông báo sử dụng localization
    public void ShowLocalizedNoti(string localizationKey, float duration = 0.5f, params object[] args)
    {
        if (LocalizationManager.instance != null && LocalizationManager.instance.isReady)
        {
            string localizedMessage = LocalizationManager.instance.GetLocalizedValue(localizationKey);
            if (args != null && args.Length > 0)
            {
                localizedMessage = string.Format(localizedMessage, args);
            }
            ShowNoti(localizedMessage, duration);
        }
        else
        {
            Debug.LogWarning("LocalizationManager not ready! Fallback to key.");
            ShowNoti(localizationKey, duration);
        }
    }


    //private bool ClickOn(GameObject obj)
    //{
    //    Vector3 mousePos = Input.mousePosition;
    //    mousePos.z = Camera.main.nearClipPlane; 
    //    Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

    //    return RectTransformUtility.RectangleContainsScreenPoint(obj.GetComponent<RectTransform>(), mousePos);
    //}

    private void HandleNextMap(bool isNext)
    {
        nextMapPanel.SetActive(false);
        Debug.Log(isNext ? "Yes" : "No");
        if (isNext)
        {
            GameManager.Instance.LoadNextMap();
            foreach (var panel in panels)
            {
                panel.SetActive(false);
            }
        }
    }

    public void EnterBagButton()
    {
        bagPanel.SetActive(true);
    }
 
    public void EnterShopButton() 
    {
        shopPanel.SetActive(true);
    }

    public void EnterItemInShop(float cost)
    {
        currentCost = cost;
        buyPanel.SetActive(true);
    }

    public void ClosePanel(GameObject panel) // Button để đóng 1 panel
    {
 
        if (panel != null)
        {
            panel.gameObject.SetActive(false);
        }
    }

    public void HideAllPanels()
    {
        foreach (var panel in panels)
        {
            panel.SetActive(false);
        }

        EventSystem.current.SetSelectedGameObject(null); // Xóa button được chọn
    }

    public void AcceptToBuy()
    {
        if (Bag.Instance.inBagItems[1].totalQuantity >= currentCost)
        {
            Bag.Instance.inBagItems[1].totalQuantity -= currentCost;
            ShowLocalizedNoti("SHOP_BOUGHT_ITEM", 0.5f);
            GameManager.Instance.isHaveDagger = true;
            currentBuyItemButton.SetActive(false);
            buyPanel.SetActive(false);
        }
        else
        {
            ShowLocalizedNoti("SHOP_INSUFFICIENT_COINS", 0.5f);
        }
    }

    public void DeclineToBuy()
    {
        buyPanel.SetActive(false);
    }

    private void Update()
    {

    }
}

