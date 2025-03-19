using System.Collections.Generic;
using UnityEngine;


public class TurnCommandProcessor : ITurnCommandProcessor
{
    private readonly CommandQueue _commandQueue;

    public TurnCommandProcessor(CommandQueue commandQueue)
    {
        _commandQueue = commandQueue;
    }

    public void ProcessDiceRoll(IPlayer player, List<Vector3> tiles)
    {
        _commandQueue.EnqueueCommand(new WaitCommand(player, 1, _commandQueue));
        _commandQueue.EnqueueCommand(new MoveCommand(player, tiles, _commandQueue));
        _commandQueue.EnqueueCommand(new WaitCommand(player, 1, _commandQueue));
    }

    public void ProcessQuizRequest(IPlayer player, QuizType quizType, string sceneName)
    {
        _commandQueue.EnqueueCommand(new QuizCommand(quizType, sceneName));
        _commandQueue.EnqueueCommand(new WaitCommand(player, 1, _commandQueue));
    }

    public void ProcessQuizFinished()
    {
        _commandQueue.CommandFinished();
    }
}
