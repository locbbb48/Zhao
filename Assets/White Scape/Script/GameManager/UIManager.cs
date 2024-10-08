/*    - Codeby Bui Thanh Loc -
    contact : builoc08042004@gmail.com
*/

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

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

    public void ShowDamage(float damageAmount, Vector3 position)
    {
        // Lấy một instance của DamageText từ Object Pool
        TextMesh damageText = ShowDameTextPool.Instance.GetObject();
        damageText.transform.position = position;
        damageText.text = damageAmount.ToString();
        damageText.color = Color.red;
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

    public void ShowNoti(string message, float duration = 0.5f)
    {
        notificationQueue.Enqueue(message);
        notificationLog.Add(message);
        if (!isShowingNotification)
        {
            StartCoroutine(ShowNotification(duration));
        }
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
        logText.text = ""; // Xóa text cũ
        foreach (string noti in notificationLog)
        {
            logText.text += noti + "\n"; // Hiển thị tất cả thông báo đã lưu
        }
        logPanel.SetActive(true); // Hiển thị panel chứa log
    }

    
    
    private bool ClickOn(GameObject obj)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane; 
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        return RectTransformUtility.RectangleContainsScreenPoint(obj.GetComponent<RectTransform>(), mousePos);
    }

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

    public void HideShopPanel()
    {
        foreach (var panel in panels)
        {
            panel.SetActive(false);
        }
    }

    public void AcceptToBuy()
    {
        if (Bag.Instance.inBagItems[1].totalQuantity >= currentCost)
        {
            Bag.Instance.inBagItems[1].totalQuantity -= currentCost;
            ShowNoti("Bought this Item!");
            GameManager.Instance.isHaveDagger = true;
            currentBuyItemButton.SetActive(false);
            buyPanel.SetActive(false);
        }
        else
        {
            ShowNoti("You don't have enough coin!");
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

