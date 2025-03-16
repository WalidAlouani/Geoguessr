using System.Threading.Tasks;
using UnityEngine;

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
