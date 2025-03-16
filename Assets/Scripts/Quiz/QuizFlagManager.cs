using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Zenject;

public class QuizFlagManager : MonoBehaviour
{
    [SerializeField] private AssetLabelReference jsonLabel;

    [SerializeField] private UI_QuizManagerFlag uI_QuizManager;
    [SerializeField] private CountryCodes CountryCodes;

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

        var quiz = await FlagQuiz.CreateAsync(quizData, assetLoader, CountryCodes);

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
