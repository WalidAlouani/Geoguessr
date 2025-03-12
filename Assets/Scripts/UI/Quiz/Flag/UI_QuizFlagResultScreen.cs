using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuizFlagResultScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text correctAnswerText;
    [SerializeField] private Image correctAnswerImage;
    [SerializeField] private TMP_Text tapToContinueText;
    [SerializeField] private GameObject tapToContinue;
    [SerializeField] private GameObject correct;
    [SerializeField] private GameObject wrong;

    public void SetResult(FlagQuiz quizData, bool isCorrectAnswer)
    {
        if (isCorrectAnswer)
            correct.SetActive(true);
        else
            wrong.SetActive(true);

        correctAnswerImage.sprite = quizData.Answers[quizData.CorrectAnswerIndex];
        correctAnswerText.text = quizData.CorrectCountryName;

        StartCoroutine(ShowTapToContinue());
    }

    private IEnumerator ShowTapToContinue()
    {
        yield return new WaitForSeconds(2);
        tapToContinue.SetActive(true);
        tapToContinueText.DOFade(1, 0.5f);
    }
}
