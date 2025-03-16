using System;
using System.Collections.Generic;

public class CommandQueue
{
    private Queue<ICommand> commandQueue = new Queue<ICommand>();
    private bool isExecuting = false;
    public Action OnQueueEmpty;

    public void EnqueueCommand(ICommand command)
    {
        commandQueue.Enqueue(command);
        if (!isExecuting)
        {
            ExecuteNextCommand();
        }
    }

    private void ExecuteNextCommand()
    {
        if (commandQueue.Count > 0)
        {
            isExecuting = true;
            ICommand command = commandQueue.Dequeue();
            command.Execute();
        }
        else
        {
            isExecuting = false;
            OnQueueEmpty?.Invoke();
        }
    }

    public void CommandFinished()
    {
        ExecuteNextCommand();
    }
}
