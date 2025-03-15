using DG.Tweening;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private float rotationDuration = 0.5f;
    [SerializeField] private float delayBetweenLoops = 5f;
    [SerializeField] private Vector3 rotationAmount = new Vector3(0, 360f, 0);
    [SerializeField] private Ease rotationEase = Ease.Linear;

    [SerializeField] private float scaleDuration = 0.55f;
    [SerializeField] private float scaleValue = 1.2f;

    [SerializeField] private float moveDuration = 0.25f;
    [SerializeField] private float moveValue = 0.5f;

    [SerializeField] private Ease moveEase = Ease.Linear;

    private Sequence loopSequence;
    private Sequence moveSequence;

    private Quaternion initialRoation;
    private Vector3 initialPosition;

    public void Init()
    {
        initialRoation = transform.localRotation;
        initialPosition = transform.localPosition;

        CreateLoopingScaleSequence();
        CreateLoopingRotationSequence();
        CreateMovingSequence();
    }

    private void CreateLoopingScaleSequence()
    {
        Sequence scaleSequence = DOTween.Sequence();

        scaleSequence.Join(transform.DOScaleX(scaleValue, scaleDuration));
        scaleSequence.Join(transform.DOScaleY(scaleValue, scaleDuration));

        scaleSequence.SetLoops(-1, LoopType.Yoyo);

        scaleSequence.Play();
    }

    private void CreateLoopingRotationSequence()
    {
        loopSequence = DOTween.Sequence().SetAutoKill(false);

        loopSequence.AppendInterval(delayBetweenLoops);

        loopSequence.Append(transform.DORotate(transform.rotation.eulerAngles + rotationAmount, rotationDuration, RotateMode.FastBeyond360)
                                       .SetEase(rotationEase));

        loopSequence.SetLoops(-1, LoopType.Restart);

        loopSequence.Play();
    }

    private void CreateMovingSequence()
    {
        moveSequence = DOTween.Sequence().SetAutoKill(false);

        moveSequence.Append(transform.DOMoveY(moveValue, moveDuration * 0.5f).SetEase(moveEase));
        moveSequence.Append(transform.DOMoveY(initialPosition.y, moveDuration * 0.5f).SetEase(moveEase));
    }

    public void PlayLoopingRotation(bool play)
    {
        if (play)
        {
            loopSequence.Play();
        }
        else
        {
            loopSequence.Pause();
            transform.localRotation = initialRoation;
        }
    }

    public void UpdateMovingTime(float percentageTimeLeft)
    {
        float elapsedTime = moveDuration * (1 - percentageTimeLeft);
        moveSequence.Goto(elapsedTime, false);
    }
}
