using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

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
        _signalBus.Subscribe<PlayerCreatedSignal>(OnPlayerCreated);
        _signalBus.Subscribe<TurnStartedSignal>(OnTurnedChanged);
        _signalBus.Subscribe<CoinsUpdateSignal>(OnCoinsUpdated);
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<PlayerCreatedSignal>(OnPlayerCreated);
        _signalBus.Unsubscribe<TurnStartedSignal>(OnTurnedChanged);
        _signalBus.Unsubscribe<CoinsUpdateSignal>(OnCoinsUpdated);
    }

    private void OnPlayerCreated(PlayerCreatedSignal signal)
    {
        var newIndex = signal.Player.Index;
        var playerUI = Instantiate(playerTopBarPrefab, spawnParent);
        playerUI.transform.localPosition = newIndex == 0 ? Vector3.zero : Vector3.up * 1000;
        playersUI[newIndex] = playerUI;
    }

    private void OnTurnedChanged(TurnStartedSignal signal)
    {
        var newIndex = signal.Player.Index;
        if (current == newIndex)
            return;

        if (playersUI.TryGetValue(current, out var playerUI))
        {
            playerUI.transform.DOLocalMoveY(1000, 0.3f).SetDelay(1.5f);
        }
        current = newIndex;
        playersUI[current].transform.DOLocalMoveY(0, 0.3f).SetDelay(1.5f);
    }

    private void OnCoinsUpdated(CoinsUpdateSignal signal)
    {
        playersUI[signal.Player.Index].UpdateCoins(signal.Player.Coins);
    }
}
