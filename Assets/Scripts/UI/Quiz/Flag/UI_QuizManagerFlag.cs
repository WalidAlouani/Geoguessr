using System.Collections;
using UnityEngine;

public class UI_QuizManagerFlag : MonoBehaviour
{
    [SerializeField] private UI_QuizFlagQuestionScreen question; //Maybe IQuizView
    [SerializeField] private UI_QuizFlagResultScreen result;

    private QuizFlagManager _quizManager;
    private FlagQuiz _quizData;

    private void OnEnable()
    {
        question.OnAnswered += OnQuizAnswered;
    }

    private void OnDisable()
    {
        question.OnAnswered -= OnQuizAnswered;
    }

    public void Initialize(QuizFlagManager quizManager, FlagQuiz quizData)
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
        result.Initialize(_quizManager, _quizData, quizResult);
    }
}
