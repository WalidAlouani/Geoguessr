using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class QuizFlagManager : QuizManagerBase<FlagQuiz, Sprite>
{
    [SerializeField] private UI_QuizManagerFlag uI_QuizManager;
    [SerializeField] private CountryCodes CountryCodes;
    [SerializeField] private AssetLabelReference assetLabel;
    protected override IQuizUIManager<FlagQuiz, Sprite> UIQuizManager => uI_QuizManager;
    protected FlagQuizGenerator FlagQuizGenerator;

    private void Awake()
    {
        FlagQuizGenerator = new FlagQuizGenerator(CountryCodes, assetLabel);
    }

    protected override async UniTask<FlagQuiz> CreateQuizAsync(QuizData quizData)
    {
        quizData = await FlagQuizGenerator.Generate();
        return await FlagQuiz.CreateAsync(quizData, _assetLoader, CountryCodes);
    }
}
