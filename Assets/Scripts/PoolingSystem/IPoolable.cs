using System;

public interface IPoolable
{
    Action<IPoolable> Return { get; set; }

    void OnCreate();

    void OnRelease();
}

