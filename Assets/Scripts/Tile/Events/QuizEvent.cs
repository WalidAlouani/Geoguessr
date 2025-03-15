using UnityEngine;
using System;

public class QuizEvent : ITileEvent
{
    //private QuizUI quizUI;

    //public QuizEvent(QuizUI quizUI)
    //{
    //    this.quizUI = quizUI;
    //}

    public void Execute(Player player, Action onEventComplete)
    {
        //quizUI.ShowQuizForPlayer(player, onEventComplete);
    }
}
