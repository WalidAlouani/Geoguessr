using System.Collections;
using UnityEngine;

public class UI_QuizManagerFlag : MonoBehaviour
{
    [SerializeField] private UI_QuizFlagQuestionScreen question; //Maybe IQuizView
    [SerializeField] private UI_QuizFlagResultScreen result;

    private FlagQuiz quizData;

    private void OnEnable()
    {
        question.OnAnswered += OnQuizAnswered;
    }

    private void OnDisable()
    {
        question.OnAnswered -= OnQuizAnswered;
    }

    public void Initialize(FlagQuiz quizData)
    {
        this.quizData = quizData;
        result.gameObject.SetActive(false);
        question.gameObject.SetActive(true);
        question.DisplayQuiz(quizData);
    }

    private void OnQuizAnswered(bool correctAnswer)
    {
        StartCoroutine(ShowResultScreen(correctAnswer));
    }

    private IEnumerator ShowResultScreen(bool correctAnswer)
    {
        yield return new WaitForSeconds(1.5f);
        result.gameObject.SetActive(true);
        question.gameObject.SetActive(false);
        result.SetResult(quizData, correctAnswer);
    }
}
