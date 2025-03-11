using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_QuizResultScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text correctAnswer;
    [SerializeField] private TMP_Text tapToContinue;
    [SerializeField] private GameObject correct;
    [SerializeField] private GameObject wrong;

    public void SetResult(QuizData quizData, bool correctAnswer)
    {
        if (correctAnswer)
            correct.SetActive(true);
        else
            wrong.SetActive(true);

        this.correctAnswer.text = quizData.Answers[quizData.CorrectAnswerIndex].Text.ToString();

        tapToContinue.DOFade(1, 0.5f).SetDelay(2);
    }



    // show tap to continue
}
