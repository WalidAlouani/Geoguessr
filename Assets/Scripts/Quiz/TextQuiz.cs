using System.Threading.Tasks;
using UnityEngine;

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
