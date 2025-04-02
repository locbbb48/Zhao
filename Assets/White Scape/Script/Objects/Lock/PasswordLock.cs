/*	  - Codeby Bui Thanh Loc -
	contact : builoc08042004@gmail.com
*/

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PasswordLock : LockBase
{
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private Button okButton;
    [SerializeField] private TextMeshProUGUI placeholderText;

    [SerializeField] private string passwordKey; // Key để lấy mật khẩu từ JSON

    private string correctPassword;

    private void Start()
    {
        correctPassword = LocalizationManager.instance.GetLocalizedValue(passwordKey);

        // Tạo Placeholder dạng "_____" dựa trên độ dài mật khẩu
        placeholderText.text = new string('_', correctPassword.Length);

        passwordInputField.characterLimit = correctPassword.Length;

        okButton.onClick.AddListener(TryUnlock);

        passwordInputField.onSubmit.AddListener(delegate { TryUnlock(); });
    }

    private void OnEnable()
    {
        passwordInputField.text = "";
        passwordInputField.ActivateInputField(); 
        EventSystem.current.SetSelectedGameObject(passwordInputField.gameObject);
    }

    protected override void TryUnlock()
    {
        if(UIManager.Instance.PasswordLockPanel.activeSelf == false)
        {
            UIManager.Instance.PasswordLockPanel.SetActive(true);
            passwordInputField.ActivateInputField();
            EventSystem.current.SetSelectedGameObject(okButton.gameObject);
        }
        else
        {
            string enteredPassword = passwordInputField.text.ToUpper();

            if (enteredPassword == correctPassword)
            {
                UIManager.Instance.ShowLocalizedNoti("LOCK_UNLOCKED", 0.5f);
                UIManager.Instance.PasswordLockPanel.SetActive(false);
                gameObject.SetActive(false);
            }
            else
            {
                UIManager.Instance.ShowLocalizedNoti("LOCK_INCORRECT_PASSWORD", 0.5f,enteredPassword);
                passwordInputField.text = ""; // Reset nếu nhập sai
                passwordInputField.ActivateInputField();
            }
        }
    }
}

