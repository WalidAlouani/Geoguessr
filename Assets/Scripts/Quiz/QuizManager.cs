using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    public TextAsset quizFile;
    public UI_QuizManager uI_QuizManager;


    void Start()
    {
        var quizData = JsonConvert.DeserializeObject<QuizData>(quizFile.text);
        uI_QuizManager.DisplayQuiz(quizData);
    }
}
