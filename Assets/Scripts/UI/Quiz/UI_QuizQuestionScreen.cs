using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuizQuestionScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text question;
    [SerializeField] private Image image;
    [SerializeField] private UI_ButtonAnswer[] answers;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private int time = 15;

    private QuizData quizData;
    private SimpleTimer timer;
    private IAssetLoader assetLoader;

    public event Action<bool> OnAnswered;

    private void Awake()
    {
        timer = new SimpleTimer(time);
    }

    private void OnEnable()
    {
        for (int i = 0; i < answers.Length; i++)
            answers[i].OnClicked += OnAnswerClicked;

        timer.OnSecondChanged += OnTimeChanged;
        timer.OnTimerFinished += OnTimerFinished;
    }

    private void OnDisable()
    {
        for (int i = 0; i < answers.Length; i++)
            answers[i].OnClicked -= OnAnswerClicked;

        timer.OnSecondChanged -= OnTimeChanged;
        timer.OnTimerFinished -= OnTimerFinished;
    }

    public async void DisplayQuiz(QuizData quizData, IAssetLoader assetLoader)
    {
        this.quizData = quizData;
        this.assetLoader = assetLoader;

        question.text = quizData.Question;
        for (int i = 0; i < quizData.Answers.Count; i++)
        {
            var answer = quizData.Answers[i];
            answers[i].Initialize(answer, i);
        }

        image.sprite = await assetLoader.LoadAssetAsync<Sprite>(quizData.CustomImageID);
        EnableAnswers(false);
    }

    public void StartQuiz()
    {
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

    private void OnAnswerClicked(int index)
    {
        var correctAnswer = index == quizData.CorrectAnswerIndex;
        answers[index].SetResponse(correctAnswer);

        EnableAnswers(false);
        timer.StopTimer();
        OnAnswered?.Invoke(correctAnswer);
    }

    private void EnableAnswers(bool enable)
    {
        for (int i = 0; i < quizData.Answers.Count; i++)
        {
            var answer = quizData.Answers[i];
            answers[i].EnableClick(enable);
        }
    }

    private void Update()
    {
        timer?.Tick(Time.deltaTime);
    }

    private void OnDestroy()
    {
        assetLoader.ReleaseAsset(quizData.CustomImageID);
    }
}
