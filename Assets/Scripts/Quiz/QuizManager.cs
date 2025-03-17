using System.Threading.Tasks;
using UnityEngine;

public class QuizTextManager : QuizManagerBase<TextQuiz, string>
{
    [SerializeField] private UI_QuizManager uI_QuizManager;

    protected override IQuizUIManager<TextQuiz, string> UIQuizManager => uI_QuizManager;

    protected override async Task<TextQuiz> CreateQuizAsync(QuizData quizData)
    {
        return await TextQuiz.CreateAsync(quizData, _assetLoader);
    }
}
