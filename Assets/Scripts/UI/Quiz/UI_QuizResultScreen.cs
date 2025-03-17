using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuizResultScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text correctAnswerText;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text tapToContinueText;
    [SerializeField] private GameObject tapToContinue;
    [SerializeField] private GameObject correct;
    [SerializeField] private GameObject wrong;
    [SerializeField] private GameObject timeIsUp;
    [SerializeField] private Button continueButton;

    public void Initialize(QuizTextManager quizManager, TextQuiz quizData, QuizResult quizResult)
    {
        switch (quizResult)
        {
            case QuizResult.Correct:
                correct.SetActive(true);
                coinText.text = quizManager.RightAnswerReward.ToString();
                break;
            case QuizResult.Wrong:
                wrong.SetActive(true);
                coinText.text = quizManager.WrongAnswerReward.ToString();
                break;
            case QuizResult.TimeIsUp:
                timeIsUp.SetActive(true);
                coinText.text = quizManager.WrongAnswerReward.ToString();
                break;
        }

        correctAnswerText.text = quizData.Answers[quizData.CorrectAnswerIndex];
        continueButton.onClick.AddListener(() => quizManager.OnQuizFinished(quizResult));

        StartCoroutine(ShowTapToContinue());
    }

    private IEnumerator ShowTapToContinue()
    {
        yield return new WaitForSeconds(2);
        tapToContinue.SetActive(true);
        tapToContinueText.DOFade(1, 0.5f);
    }
}
