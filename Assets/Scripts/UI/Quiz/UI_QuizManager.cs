using System.Collections;
using UnityEngine;

public class UI_QuizManager : MonoBehaviour, IQuizUIManager<TextQuiz, string>
{
    [SerializeField] private UI_QuizQuestionScreen question; //Maybe IQuizScreen
    [SerializeField] private UI_QuizResultScreen result;

    private QuizManagerBase<TextQuiz, string> _quizManager;
    private TextQuiz _quizData;

    private void OnEnable()
    {
        question.OnAnswered += OnQuizAnswered;
    }

    private void OnDisable()
    {
        question.OnAnswered -= OnQuizAnswered;
    }

    public void Initialize(QuizManagerBase<TextQuiz, string> quizManager, TextQuiz quizData)
    {
        _quizManager = quizManager;
        _quizData = quizData;
        result.gameObject.SetActive(false);
        question.gameObject.SetActive(true);
        question.DisplayQuiz(quizData);
    }

    private void OnQuizAnswered(QuizResult quizResult)
    {
        StartCoroutine(ShowResultScreen(quizResult));
    }

    private IEnumerator ShowResultScreen(QuizResult quizResult)
    {
        yield return new WaitForSeconds(1.5f);
        result.gameObject.SetActive(true);
        question.gameObject.SetActive(false);
        result.Initialize((QuizTextManager)_quizManager, _quizData, quizResult);
    }
}
