using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class UI_ButtonRandom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;

    private bool isInside = false;

    private WaitForSeconds wait = new WaitForSeconds(0.02f);

    //subscribe to game state
    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void OnEnable()
    {
        _signalBus.Subscribe<RollDiceSignal>(OnDiceRollRequested);
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<RollDiceSignal>(OnDiceRollRequested);
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
        _signalBus.Fire<RollDiceSignal>();
    }

    public void OnDiceRollRequested()
    {
        StartCoroutine(ThrowDiceCoroutine());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isInside)
            return;

        image.DOFade(1, 0.15f);
        text.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        image.DOFade(0, 0.15f);
    }

    private IEnumerator ThrowDiceCoroutine()
    {
        //button.interactable = false;
        text.gameObject.SetActive(true);

        for (int i = 0; i < 10; i++)
        {
            text.text = i.ToString();
            yield return wait;
        }

        var steps = UnityEngine.Random.Range(1, 11);
        text.text = steps.ToString();

        _signalBus.Fire(new DiceRolledSignal(steps));
    }
}
