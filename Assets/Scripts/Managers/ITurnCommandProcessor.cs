using System.Collections.Generic;
using UnityEngine;

public interface ITurnCommandProcessor
{
    CommandQueue CommandQueue { get; }
    void ProcessDiceRoll(IPlayer player, List<Vector3> tiles);
    void ProcessQuizRequest(IPlayer player, QuizType quizType, string sceneName);
    void ProcessQuizFinished();
}
