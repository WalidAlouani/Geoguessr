using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuizFlagQuestionScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text question;
    [SerializeField] private UI_ButtonAnswerFlag[] answers;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private int time = 15;

    private FlagQuiz quizData;
    private SimpleTimer timer;

    public event Action<bool> OnAnswered;

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

    public void DisplayQuiz(FlagQuiz quizData)
    {
        this.quizData = quizData;

        question.text = quizData.Question;
        for (int i = 0; i < quizData.Answers.Length; i++)
        {
            answers[i].Initialize(quizData.Answers[i]);
        }

        StartCoroutine(StartQuiz());
    }

    public void OnAnswerClicked(int index)
    {
        var correctAnswer = index == quizData.CorrectAnswerIndex;

        answers[index].SetResponse(correctAnswer);
        if (!correctAnswer)
            answers[quizData.CorrectAnswerIndex].SetResponse(true, false);

        EnableAnswers(false);
        timer.StopTimer();
        OnAnswered?.Invoke(correctAnswer);
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
        OnAnswered?.Invoke(false);
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
