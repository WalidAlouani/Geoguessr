using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuizCommand : ICommand
{
    public QuizType QuizType { get; }

    public QuizCommand(QuizType quizType)
    {
        QuizType = quizType;
    }


    public void Execute()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }
}
