using Newtonsoft.Json;
using System;
using UnityEngine;

public class QuizFlagManager : MonoBehaviour
{
    public TextAsset quizFile;
    public UI_QuizManagerFlag uI_QuizManager;
    public event Action QuizFinished;

    private IAssetLoader assetLoader;

    void Start()
    {
        assetLoader = new AddressableLoader();

        var quizData = JsonConvert.DeserializeObject<QuizData>(quizFile.text);
        uI_QuizManager.Initialize(quizData, assetLoader);
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
