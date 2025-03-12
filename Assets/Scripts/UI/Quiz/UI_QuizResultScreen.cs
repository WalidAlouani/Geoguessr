using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuizResultScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text correctAnswerText;
    [SerializeField] private TMP_Text tapToContinueText;
    [SerializeField] private GameObject tapToContinue;
    [SerializeField] private GameObject correct;
    [SerializeField] private GameObject wrong;
    [SerializeField] private GameObject timeIsUp;

    public void SetResult(TextQuiz quizData, QuizResult quizResult)
    {
        switch (quizResult)
        {
            case QuizResult.Correct:
                correct.SetActive(true);
                break;
            case QuizResult.Wrong:
                wrong.SetActive(true);
                break;
            case QuizResult.TimeIsUp:
                timeIsUp.SetActive(true);
                break;
        }

        correctAnswerText.text = quizData.Answers[quizData.CorrectAnswerIndex];

        StartCoroutine(ShowTapToContinue());
    }

    private IEnumerator ShowTapToContinue()
    {
        yield return new WaitForSeconds(2);
        tapToContinue.SetActive(true);
        tapToContinueText.DOFade(1, 0.5f);
    }
}
