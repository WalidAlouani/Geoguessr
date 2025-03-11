using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    public TextAsset quizFile;
    public UI_QuizManager uI_QuizManager;

    private IAssetLoader assetLoader;

    void Start()
    {
        assetLoader = new AddressableLoader();

        var quizData = JsonConvert.DeserializeObject<QuizData>(quizFile.text);
        uI_QuizManager.Initialize(quizData, assetLoader);
    }

    private void OnDestroy()
    {
        assetLoader.ReleaseAll();
    }
}
