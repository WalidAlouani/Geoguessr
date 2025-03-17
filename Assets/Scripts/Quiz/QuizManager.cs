using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private AssetLabelReference jsonLabel;
    [SerializeField] private UI_QuizManager uI_QuizManager;

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

        uI_QuizManager.Initialize(quiz);
    }

    public void OnQuizFinished()
    {
        _signalBus.Fire(new QuizFinishedSignal());
        _sceneLoader.UnloadScene(gameObject.scene.name);
    }

    private void OnDestroy()
    {
        _assetLoader.ReleaseAll();
    }
}
