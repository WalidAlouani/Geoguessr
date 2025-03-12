using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class UI_QuizResultScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text correctAnswer;
    [SerializeField] private TMP_Text tapToContinueText;
    [SerializeField] private GameObject tapToContinue;
    [SerializeField] private GameObject correct;
    [SerializeField] private GameObject wrong;

    public void SetResult(QuizData quizData, bool isCorrectAnswer)
    {
        if (isCorrectAnswer)
            correct.SetActive(true);
        else
            wrong.SetActive(true);

        var answer = quizData.Answers[quizData.CorrectAnswerIndex];

        this.correctAnswer.text = answer.Text.ToString();

        StartCoroutine(ShowTapToContinue());
    }

    private IEnumerator ShowTapToContinue()
    {
        yield return new WaitForSeconds(2);
        tapToContinue.SetActive(true);
        tapToContinueText.DOFade(1, 0.5f);
    }
}
