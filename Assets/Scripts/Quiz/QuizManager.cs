using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Zenject;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private AssetLabelReference jsonLabel;

    [SerializeField] private UI_QuizManager uI_QuizManager;

    private RandomResourceKeyPicker picker = new RandomResourceKeyPicker();
    private IAssetLoader assetLoader = new AddressableLoader();

    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    async void Start()
    {
        var res = await RandomResourceKeyPicker.GetRandomResourceKeyAsync(jsonLabel);

        var quizFile = await assetLoader.LoadAssetAsync<TextAsset>(res);

        var quizData = QuizSerializerJson.LoadFromJson(quizFile.text);

        var quiz = await TextQuiz.CreateAsync(quizData, assetLoader);

        uI_QuizManager.Initialize(quiz);
    }

    public void OnQuizFinished()
    {
        _signalBus.Fire<QuizFinishedSignal>();
        SceneLoader.Instance.UnloadScene(gameObject.scene.name);
    }

    private void OnDestroy()
    {
        assetLoader.ReleaseAll();
    }
}
