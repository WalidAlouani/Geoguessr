using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private AssetLabelReference jsonLabel;
    [SerializeField] private UI_QuizManager uI_QuizManager;

    public int RightAnswerReward = 5000;
    public int WrongAnswerReward = 2000;

    private IAssetLoader _assetLoader = new AddressableLoader();
    private SignalBus _signalBus;
    private SceneLoader _sceneLoader;

    [Inject]
    public void Construct(SignalBus signalBus, SceneLoader sceneLoader)
    {
        _signalBus = signalBus;
        _sceneLoader = sceneLoader;
    }

    async void Start()
    {
        var res = await RandomResourceKeyPicker.GetRandomResourceKeyAsync(jsonLabel);

        var quizFile = await _assetLoader.LoadAssetAsync<TextAsset>(res);

        var quizData = QuizSerializerJson.LoadFromJson(quizFile.text);

        var quiz = await TextQuiz.CreateAsync(quizData, _assetLoader);

        uI_QuizManager.Initialize(this, quiz);
    }

    public void OnQuizFinished(QuizResult quizResult)
    {
        var reward = quizResult == QuizResult.Correct ? RightAnswerReward : WrongAnswerReward;
        _signalBus.Fire(new QuizFinishedSignal(reward));
        _sceneLoader.UnloadScene(gameObject.scene.name);
    }

    private void OnDestroy()
    {
        _assetLoader.ReleaseAll();
    }
}
