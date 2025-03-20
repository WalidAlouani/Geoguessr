using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Zenject;

public class UI_ButtonRandom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;

    private bool isInside = false;
    private IPlayer currentPlayerTurn;
    private WaitForSeconds wait = new WaitForSeconds(0.02f);

    private Dice _dice;
    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus, DiceFactory diceFactory)
    {
        _signalBus = signalBus;
        _dice = diceFactory.Create(0, 10);
    }

    private void OnEnable()
    {
        _signalBus.Subscribe<RollDiceSignal>(OnDiceRollRequested);
        _signalBus.Subscribe<TurnStartedSignal>(OnTurnStarted);
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<RollDiceSignal>(OnDiceRollRequested);
        _signalBus.Unsubscribe<TurnStartedSignal>(OnTurnStarted);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isInside = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isInside = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!button.interactable)
            return;

        _signalBus.Fire(new RollDiceSignal(currentPlayerTurn));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isInside)
            return;

        if (!button.interactable)
            return;

        SetState(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!button.interactable)
            return;

        image.DOFade(0, 0.15f);
    }

    private void SetState(bool active)
    {
        image.DOFade(active ? 1 : 0, 0.15f);
        text.gameObject.SetActive(!active);
    }

    private void OnDiceRollRequested()
    {
        StartCoroutine(ThrowDiceCoroutine());
    }

    private IEnumerator ThrowDiceCoroutine()
    {
        button.interactable = false;

        SetState(false);

        for (int i = 0; i < 10; i++)
        {
            text.text = i.ToString();
            yield return wait;
        }

        var steps = _dice.Roll();

        text.text = steps.ToString();
    }

    private void OnTurnStarted(TurnStartedSignal signal)
    {
        currentPlayerTurn = signal.Player;
        SetState(true);

        if (signal.Player is PlayerHumain)
            button.interactable = true;
    }
}
