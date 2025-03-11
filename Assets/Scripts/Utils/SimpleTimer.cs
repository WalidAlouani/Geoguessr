using UnityEngine;
using System;

public class SimpleTimer
{
    private float initialTimeInSeconds = 10f;
    private float currentTime;
    private bool isTimerRunning = false;
    private int lastSecond = -1;

    public event Action<int> OnSecondChanged;

    public event Action OnTimerFinished;

    public SimpleTimer(float startTimeInSeconds = 10f)
    {
        InitializeTimer(startTimeInSeconds);
    }

    public void Tick(float deltaTime)
    {
        if (!isTimerRunning)
            return;

        currentTime -= deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isTimerRunning = false;
            OnTimerFinished?.Invoke();
        }

        int currentSecond = Mathf.CeilToInt(currentTime);

        if (currentSecond != lastSecond)
        {
            lastSecond = currentSecond;
            OnSecondChanged?.Invoke(currentSecond);
        }
    }

    public void InitializeTimer(float startTimeInSeconds)
    {
        initialTimeInSeconds = Mathf.Max(0f, startTimeInSeconds);
        currentTime = initialTimeInSeconds;
        lastSecond = Mathf.CeilToInt(currentTime);
        OnSecondChanged?.Invoke(lastSecond);
    }

    public void StartTimer()
    {
        if (isTimerRunning)
            return;

        isTimerRunning = true;
        lastSecond = -1;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public void ResetTimer()
    {
        StopTimer();
        currentTime = initialTimeInSeconds;
        lastSecond = Mathf.CeilToInt(currentTime);
        OnSecondChanged?.Invoke(lastSecond);
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public float GetInitialTime()
    {
        return initialTimeInSeconds;
    }

    public bool IsRunning()
    {
        return isTimerRunning;
    }
}
