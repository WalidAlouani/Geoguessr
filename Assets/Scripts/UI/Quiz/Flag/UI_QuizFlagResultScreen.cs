using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuizFlagResultScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text correctAnswerText;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private Image correctAnswerImage;
    [SerializeField] private TMP_Text tapToContinueText;
    [SerializeField] private GameObject tapToContinue;
    [SerializeField] private GameObject correct;
    [SerializeField] private GameObject wrong;
    [SerializeField] private GameObject timeIsUp;
    [SerializeField] private Button continueButton;

    public void Initialize(QuizFlagManager quizManager, FlagQuiz quizData, QuizResult quizResult)
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

        correctAnswerImage.sprite = quizData.Answers[quizData.CorrectAnswerIndex];
        correctAnswerText.text = quizData.CorrectCountryName;
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
