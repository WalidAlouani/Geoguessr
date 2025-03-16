using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class UI_FloatingText : MonoBehaviour, IPoolable
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private TMP_Text text;
    private Sequence moveSequence;
    private Vector3 initialPosition;

    public Action<IPoolable> Return { get; set; }

    private void Awake()
    {
        canvas.worldCamera = Camera.main;
    }

    public void Init(int value)
    {
        var sign = value > 0 ? "+" : string.Empty;
        text.text = $"{sign}{value}";
        text.color = value >= 0 ? Color.blue : Color.red;

        CreateMovingSequence();
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }

    private void CreateMovingSequence()
    {
        moveSequence = DOTween.Sequence();

        text.transform.localScale = new Vector3(1, 0, 1);
        initialPosition = text.transform.localPosition;

        moveSequence.Append(text.transform.DOScaleY(1, 0.1f).SetEase(Ease.Linear));
        moveSequence.Append(text.transform.DOLocalMoveY(initialPosition.y + 25, 0.25f).SetEase(Ease.Linear));

        moveSequence.Append(text.DOFade(0, 0.15f).SetEase(Ease.Linear).SetDelay(1));

        moveSequence.OnComplete(() => Return?.Invoke(this));

        moveSequence.Play();
    }

    public void OnCreate()
    {
    }

    public void OnRelease()
    {
        text.DOFade(1, 0);
        text.transform.localPosition = initialPosition;
    }
}
