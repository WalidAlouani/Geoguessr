using Newtonsoft.Json;
using System;
using UnityEngine;

public class QuizFlagManager : MonoBehaviour
{
    public RandomResourceKeyPicker picker;
    public CountryCodes CountryCodes;
    public UI_QuizManagerFlag uI_QuizManager;
    public event Action QuizFinished;

    private TextAsset quizFile;
    private IAssetLoader assetLoader;

    async void Start()
    {
        var res = await picker.GetRandomResourceKeyAsync();

        Debug.Log(res);

        assetLoader = new AddressableLoader();

        quizFile = await assetLoader.LoadAssetAsync<TextAsset>(res);

        var quizData = JsonConvert.DeserializeObject<QuizData>(quizFile.text);

        var quiz = await FlagQuiz.CreateAsync(quizData, assetLoader, CountryCodes);

        uI_QuizManager.Initialize(quiz);
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
