using Cysharp.Threading.Tasks;
using UnityEngine;

public class QuizFlagManager : QuizManagerBase<FlagQuiz, Sprite>
{
    [SerializeField] private UI_QuizManagerFlag uI_QuizManager;
    [SerializeField] private CountryCodes CountryCodes;

    protected override IQuizUIManager<FlagQuiz, Sprite> UIQuizManager => uI_QuizManager;

    protected override async UniTask<FlagQuiz> CreateQuizAsync(QuizData quizData)
    {
        return await FlagQuiz.CreateAsync(quizData, _assetLoader, CountryCodes);
    }
}
