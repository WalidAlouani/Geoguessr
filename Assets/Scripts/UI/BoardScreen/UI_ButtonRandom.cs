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
    private WaitForSeconds wait = new WaitForSeconds(0.02f);
    private SignalBus _signalBus;

    //subscribe to game state

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
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

        _signalBus.Fire<RollDiceSignal>();
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

        var steps = Random.Range(1, 11);
        text.text = steps.ToString();

        _signalBus.Fire(new DiceRolledSignal(steps));
    }

    private void OnTurnStarted(TurnStartedSignal signal)
    {
        SetState(true);

        if (signal.Player.Type == PlayerType.Humain)
            button.interactable = true;
    }
}
