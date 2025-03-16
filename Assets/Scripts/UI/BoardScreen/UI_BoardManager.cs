using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UI_BoardManager : MonoBehaviour
{
    [SerializeField] private RectTransform top;
    [SerializeField] private RectTransform bottom;
    [SerializeField] private UI_ButtonRandom rollDiceButton;
    [SerializeField] private UI_PlayerTopBar playerTopBarPrefab;
    [SerializeField] private RectTransform spawnParent;

    private SignalBus _signalBus;
    private Dictionary<int, UI_PlayerTopBar> playersUI = new Dictionary<int, UI_PlayerTopBar>();
    private int current = -1;

    [Inject]
    public void Construct(SignalBus signalBus, CommandQueue commandQueue)
    {
        _signalBus = signalBus;
    }

    private void OnEnable()
    {
        _signalBus.Subscribe<PlayersCreatedSignal>(OnPlayersCreated);
        _signalBus.Subscribe<TurnStartedSignal>(OnTurnedChanged);
        _signalBus.Subscribe<CoinsUpdateSignal>(OnCoinsUpdated);

        _signalBus.Subscribe<QuizRequestedSignal>(OnQuizRequested);
        _signalBus.Subscribe<QuizFinishedSignal>(OnQuizFinished);
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<PlayersCreatedSignal>(OnPlayersCreated);
        _signalBus.Unsubscribe<TurnStartedSignal>(OnTurnedChanged);
        _signalBus.Unsubscribe<CoinsUpdateSignal>(OnCoinsUpdated);

        _signalBus.Unsubscribe<QuizRequestedSignal>(OnQuizRequested);
        _signalBus.Unsubscribe<QuizFinishedSignal>(OnQuizFinished);
    }

    private void OnPlayersCreated(PlayersCreatedSignal signal)
    {
        for (int i = 0; i < signal.Players.Count; i++)
        {
            var newIndex = signal.Players[i].Index;
            var playerUI = Instantiate(playerTopBarPrefab, spawnParent);
            playerUI.Init(newIndex, signal.Players.Count > 1);
            playersUI[newIndex] = playerUI;
        }
    }

    private void OnTurnedChanged(TurnStartedSignal signal)
    {
        var newIndex = signal.Player.Index;
        if (current == newIndex)
            return;

        if (playersUI.TryGetValue(current, out var playerUI))
        {
            playerUI.transform.DOLocalMoveY(1000, 0.3f).SetDelay(0.5f);
        }
        current = newIndex;
        playersUI[current].transform.DOLocalMoveY(0, 0.3f).SetDelay(0.5f);
    }

    private void OnCoinsUpdated(CoinsUpdateSignal signal)
    {
        playersUI[signal.Player.Index].UpdateCoins(signal.Player.Coins);
    }

    private void OnQuizRequested()
    {
        top.GetComponent<Animator>().SetBool("On", false);
        bottom.GetComponent<Animator>().SetBool("On", false);
    }

    private void OnQuizFinished()
    {
        top.GetComponent<Animator>().SetBool("On", true);
        bottom.GetComponent<Animator>().SetBool("On", true);
    }
}
