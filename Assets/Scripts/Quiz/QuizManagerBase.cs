using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public abstract class QuizManagerBase<TQuiz, TData> : MonoBehaviour
    where TQuiz : QuizDataBase<TData>
{
    [SerializeField] private AssetLabelReference jsonLabel;
    public int RightAnswerReward = 5000;
    public int WrongAnswerReward = 2000;

    protected IAssetLoader _assetLoader;
    protected SignalBus _signalBus;
    protected SceneLoader _sceneLoader;

    protected abstract IQuizUIManager<TQuiz, TData> UIQuizManager { get; }

    [Inject]
    public void Construct(SignalBus signalBus, SceneLoader sceneLoader, IAssetLoader assetLoader)
    {
        _signalBus = signalBus;
        _sceneLoader = sceneLoader;
        _assetLoader = assetLoader;
    }

    async void Start()
    {
        var res = await RandomResourceKeyPicker.GetRandomResourceKeyAsync(jsonLabel);

        var quizFile = await _assetLoader.LoadAssetAsync<TextAsset>(res);

        QuizData quizData = QuizSerializerJson.LoadFromJson(quizFile.text);

        TQuiz quiz = await CreateQuizAsync(quizData);

        UIQuizManager.Initialize(this, quiz);
    }

    protected abstract UniTask<TQuiz> CreateQuizAsync(QuizData quizData);

    public void OnQuizFinished(QuizResult quizResult)
    {
        int reward = quizResult == QuizResult.Correct ? RightAnswerReward : WrongAnswerReward;
        _signalBus.Fire(new QuizFinishedSignal(reward));
        _sceneLoader.UnloadScene(gameObject.scene.name);
    }

    protected virtual void OnDestroy()
    {
        _assetLoader.ReleaseAll();
    }
}



