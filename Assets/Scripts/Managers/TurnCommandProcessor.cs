using System.Collections.Generic;
using UnityEngine;


public class TurnCommandProcessor : ITurnCommandProcessor
{
    public CommandQueue CommandQueue { get; }

    public TurnCommandProcessor(CommandQueue commandQueue)
    {
        CommandQueue = commandQueue;
    }

    public void ProcessDiceRoll(IPlayer player, List<Vector3> tiles)
    {
        CommandQueue.EnqueueCommand(new WaitCommand(player, 1, CommandQueue));
        CommandQueue.EnqueueCommand(new MoveCommand(player, tiles, CommandQueue));
        CommandQueue.EnqueueCommand(new WaitCommand(player, 1, CommandQueue));
    }

    public void ProcessQuizRequest(IPlayer player, QuizType quizType, string sceneName)
    {
        CommandQueue.EnqueueCommand(new QuizCommand(quizType, sceneName));
        CommandQueue.EnqueueCommand(new WaitCommand(player, 1, CommandQueue));
    }

    public void ProcessQuizFinished()
    {
        CommandQueue.CommandFinished();
    }
}
