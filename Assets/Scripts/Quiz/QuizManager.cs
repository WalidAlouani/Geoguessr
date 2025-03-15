using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public TextAsset quizFile;
    public UI_QuizManager uI_QuizManager;
    public event Action QuizFinished;

    private IAssetLoader assetLoader;

    async void Start()
    {
        assetLoader = new AddressableLoader();

        var quizData = JsonConvert.DeserializeObject<QuizData>(quizFile.text);

        var quiz = await TextQuiz.CreateAsync(quizData, assetLoader);

        uI_QuizManager.Initialize(quiz);
    }

    public void OnQuizFinished()
    {
        QuizFinished?.Invoke();
        Debug.Log("Exit");
        SceneManager.UnloadSceneAsync("QuizText");
    }

    private void OnDestroy()
    {
        assetLoader.ReleaseAll();
    }
}

public class QuizFactory
{
    public static async Task<QuizDataBase<TAnswer>> CreateQuizAsync<TAnswer>(QuizData quizData, IAssetLoader assetLoader, ICountryCodeLookup countryCodeLookup)
    {
        switch (quizData.QuestionType)
        {
            case QuestionType.Text:
                if (typeof(TAnswer) != typeof(string))
                    throw new ArgumentException("For a text quiz, the generic type TAnswer must be string.");
                // Create the text quiz and cast it to QuizDataBase<TAnswer>
                var textQuiz = await TextQuiz.CreateAsync(quizData, assetLoader);
                return textQuiz as QuizDataBase<TAnswer>;

            case QuestionType.Flag:
                if (typeof(TAnswer) != typeof(Sprite))
                    throw new ArgumentException("For a flag quiz, the generic type TAnswer must be Sprite.");
                var flagQuiz = await FlagQuiz.CreateAsync(quizData, assetLoader, countryCodeLookup);
                return flagQuiz as QuizDataBase<TAnswer>;

            default:
                throw new NotSupportedException($"Quiz type {quizData.QuestionType} is not supported.");
        }
    }
}


public abstract class QuizDataBase<TAnswer>
{
    public string Question;
    public int CorrectAnswerIndex;
    public TAnswer[] Answers;

    protected QuizDataBase(QuizData quizData)
    {
        Question = quizData.Question;
        CorrectAnswerIndex = quizData.CorrectAnswerIndex;
    }
}

public class TextQuiz : QuizDataBase<string>
{
    public Sprite ReferenceImage;

    public static async Task<TextQuiz> CreateAsync(QuizData quizData, IAssetLoader assetLoader)
    {
        var instance = new TextQuiz(quizData);
        await instance.InitializeAsync(quizData, assetLoader);
        return instance;
    }

    private async Task InitializeAsync(QuizData quizData, IAssetLoader assetLoader)
    {
        ReferenceImage = await assetLoader.LoadAssetAsync<Sprite>(quizData.CustomImageID);
    }

    public TextQuiz(QuizData quizData) : base(quizData)
    {
        var answersCount = quizData.Answers.Count;
        Answers = new string[answersCount];
        for (int i = 0; i < answersCount; i++)
        {
            Answers[i] = quizData.Answers[i].Text;
        }
    }
}

public class FlagQuiz : QuizDataBase<Sprite>
{
    public string CorrectCountryName;

    public static async Task<FlagQuiz> CreateAsync(QuizData quizData, IAssetLoader assetLoader, ICountryCodeLookup countryCodeLookup)
    {
        var instance = new FlagQuiz(quizData);
        await instance.InitializeAsync(quizData, assetLoader, countryCodeLookup);
        return instance;
    }

    private async Task InitializeAsync(QuizData quizData, IAssetLoader assetLoader, ICountryCodeLookup countryCodeLookup)
    {
        var answersCount = quizData.Answers.Count;
        Answers = new Sprite[answersCount];
        for (int i = 0; i < answersCount; i++)
        {
            var imageID = quizData.Answers[i].ImageID;
            Answers[i] = await assetLoader.LoadAssetAsync<Sprite>(imageID);
            if (quizData.CorrectAnswerIndex == i)
                CorrectCountryName = countryCodeLookup.GetCountryName(imageID);
        }
    }

    public FlagQuiz(QuizData quizData) : base(quizData)
    {

    }
}
