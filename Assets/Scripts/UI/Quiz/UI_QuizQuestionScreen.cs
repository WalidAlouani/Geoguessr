using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum QuizResult { Correct, Wrong, TimeIsUp }

public class UI_QuizQuestionScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text question;
    [SerializeField] private Image image;
    [SerializeField] private UI_ButtonAnswer[] answers;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private int time = 15;

    private TextQuiz quizData;
    private SimpleTimer timer;

    public event Action<QuizResult> OnAnswered;

    private void Awake()
    {
        timer = new SimpleTimer(time);
    }

    private void OnEnable()
    {
        timer.OnSecondChanged += OnTimeChanged;
        timer.OnTimeIsUp += OnTimerFinished;
    }

    private void OnDisable()
    {
        timer.OnSecondChanged -= OnTimeChanged;
        timer.OnTimeIsUp -= OnTimerFinished;
    }

    public void DisplayQuiz(TextQuiz quizData)
    {
        this.quizData = quizData;

        question.text = quizData.Question;
        for (int i = 0; i < quizData.Answers.Length; i++)
        {
            var answer = quizData.Answers[i];
            answers[i].Initialize(answer);
        }

        image.sprite = quizData.ReferenceImage;

        StartCoroutine(StartQuiz());
    }

    public void OnAnswerClicked(int index)
    {
        EnableAnswers(false);
        timer.StopTimer();

        var correctAnswer = index == quizData.CorrectAnswerIndex;
        answers[index].SetResponse(correctAnswer);
        OnAnswered?.Invoke(correctAnswer ? QuizResult.Correct : QuizResult.Wrong);
    }

    private IEnumerator StartQuiz()
    {
        EnableAnswers(false);
        yield return new WaitForSeconds(1.0f);
        EnableAnswers(true);
        timer.StartTimer();
    }

    private void OnTimeChanged(int time)
    {
        timerText.text = $"{time}s";
    }

    private void OnTimerFinished()
    {
        EnableAnswers(false);
        OnAnswered?.Invoke(QuizResult.TimeIsUp);
    }

    private void EnableAnswers(bool enable)
    {
        for (int i = 0; i < quizData.Answers.Length; i++)
        {
            var answer = quizData.Answers[i];
            answers[i].EnableClick(enable);
        }
    }

    private void Update()
    {
        timer?.Tick(Time.deltaTime);
    }
}
