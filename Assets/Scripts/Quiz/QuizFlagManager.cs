using Newtonsoft.Json;
using System;
using UnityEngine;

public class QuizFlagManager : MonoBehaviour
{
    public CountryCodes CountryCodes;
    public TextAsset quizFile;
    public UI_QuizManagerFlag uI_QuizManager;
    public event Action QuizFinished;

    private IAssetLoader assetLoader;

    async void Start()
    {
        assetLoader = new AddressableLoader();

        var quizData = JsonConvert.DeserializeObject<QuizData>(quizFile.text);

        var quiz = await QuizFactory.CreateQuizAsync(quizData, assetLoader, CountryCodes);

        uI_QuizManager.Initialize((FlagQuiz)quiz);
    }

    public void OnQuizFinished()
    {
        QuizFinished?.Invoke();
        Debug.Log("Exit");
    }

    private void OnDestroy()
    {
        assetLoader.ReleaseAll();
    }
}
