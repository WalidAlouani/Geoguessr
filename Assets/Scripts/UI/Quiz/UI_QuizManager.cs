using System;
using System.Collections;
using UnityEngine;

public class UI_QuizManager : MonoBehaviour
{
    [SerializeField] private UI_QuizQuestionScreen question; //Maybe IQuizScreen
    [SerializeField] private UI_QuizResultScreen result;
    private QuizData quizData;

    private void OnEnable()
    {
        question.OnAnswered += OnQuizAnswered;
    }

    private void OnDisable()
    {
        question.OnAnswered -= OnQuizAnswered;
    }

    public void Initialize(QuizData quizData, IAssetLoader assetLoader)
    {
        this.quizData = quizData;
        result.gameObject.SetActive(false);
        question.gameObject.SetActive(true);
        question.DisplayQuiz(quizData, assetLoader);

        StartCoroutine(StartQuiz());
    }

    public IEnumerator StartQuiz()
    {
        yield return new WaitForSeconds(1.0f);
        question.StartQuiz();
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
