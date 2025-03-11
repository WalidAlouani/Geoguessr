using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuizManager : MonoBehaviour
{
    [SerializeField] private TMP_Text question;
    [SerializeField] private Image image;
    [SerializeField] private UI_ButtonAnswer[] answers;

    private QuizData quizData;

    private void OnEnable()
    {
        for (int i = 0; i < answers.Length; i++)
            answers[i].OnClicked += OnAnswerClicked;
    }

    private void OnDisable()
    {
        for (int i = 0; i < answers.Length; i++)
            answers[i].OnClicked -= OnAnswerClicked;
    }

    public async void DisplayQuiz(QuizData quizData)
    {
        this.quizData = quizData;

        question.text = quizData.Question;
        for (int i = 0; i < quizData.Answers.Count; i++)
        {
            var answer = quizData.Answers[i];
            answers[i].Initialize(answer, i);
        }

        image.sprite = await AddressableLoader.LoadAssetAsync<Sprite>(quizData.CustomImageID);
    }

    private void OnAnswerClicked(int index)
    {
        var correctAnswer = index == quizData.CorrectAnswerIndex;
        answers[index].SetResponse(correctAnswer);

        for (int i = 0; i < quizData.Answers.Count; i++)
        {
            var answer = quizData.Answers[i];
            answers[i].DisableClick();
        }
    }

    private void OnDestroy()
    {
        AddressableLoader.ReleaseAsset(quizData.CustomImageID);
    }
}
