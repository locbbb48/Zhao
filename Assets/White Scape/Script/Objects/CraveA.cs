/*    - Codeby Bui Thanh Loc -  
    contact : builoc08042004@gmail.com  
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

[System.Serializable]
public class QuestionData
{
    public string question;
    public List<string> answers;
    public string correctAnswer;
}

[System.Serializable]
public class QuestionList
{
    public List<QuestionData> questions;
}

public class CraveA : InteractedItem
{
    [SerializeField] private GameObject QuestionPanel;
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Button[] answerButtons;
    private List<QuestionData> questions;
    private int currentQuestionIndex;

    private void Awake()
    {
        QuestionPanel.SetActive(false);
        LoadQuestionsFromJson();
        SetupQuestion();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.gameObject == GameManager.Instance.player.gameObject)
        {
            QuestionPanel.SetActive(true);
        }
    }

    // Tải câu hỏi từ tệp JSON
    private void LoadQuestionsFromJson()
    {
        string jsonFilePath = Path.Combine(Application.streamingAssetsPath, "questions.json");
        string jsonContent = File.ReadAllText(jsonFilePath);
        // Sử dụng lớp QuestionList để parse file JSON
        QuestionList questionList = JsonUtility.FromJson<QuestionList>(jsonContent);
        questions = questionList.questions;
    }

    // Hiển thị câu hỏi hiện tại lên panel
    private void SetupQuestion()
    {
        // Chọn ngẫu nhiên chỉ số của câu hỏi từ 0 đến số lượng câu hỏi - 1
        currentQuestionIndex = Random.Range(0, questions.Count);

        // Lấy câu hỏi hiện tại từ danh sách
        QuestionData currentQuestion = questions[currentQuestionIndex];

        // Hiển thị câu hỏi
        questionText.text = currentQuestion.question;

        // Hiển thị các đáp án lên các button
        for (int i = 0; i < Mathf.Min(answerButtons.Length, currentQuestion.answers.Count); i++)
        {
            string answer = currentQuestion.answers[i];
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = answer;

            // Gắn sự kiện click cho từng button
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => CheckAnswer(answer));
        }
    }

    // Kiểm tra xem đáp án có đúng không
    private void CheckAnswer(string selectedAnswer)
    {
        // Nếu người chơi chọn "No", hiện thông báo nhắc nhớ câu hỏi.
        if (selectedAnswer == "No")
        {
            UIManager.Instance.ShowLocalizedNoti("QA_REMEMBER", 0.5f);
        }

        QuestionData currentQuestion = questions[currentQuestionIndex];
        if (selectedAnswer == currentQuestion.correctAnswer)
        {
            UIManager.Instance.ShowLocalizedNoti("QA_CORRECT", 0.5f);
        }
        else
        {
            UIManager.Instance.ShowLocalizedNoti("QA_INCORRECT", 0.5f);
        }

        // Ẩn panel sau khi trả lời
        QuestionPanel.SetActive(false);
    }
}
