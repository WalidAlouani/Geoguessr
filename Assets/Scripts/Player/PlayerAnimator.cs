using DG.Tweening;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private float rotationDuration = 0.5f;
    [SerializeField] private float delayBetweenLoops = 5f;
    [SerializeField] private Vector3 rotationAmount = new Vector3(0, 360f, 0);
    [SerializeField] private Ease rotationEase = Ease.Linear;

    void Start()
    {
        CreateLoopingScaleSequence();

        CreateLoopingRotationSequence();
    }

    void CreateLoopingScaleSequence()
    {
        Sequence scaleSequence = DOTween.Sequence();

        scaleSequence.Join(transform.DOScaleX(1.2f, 0.55f));
        scaleSequence.Join(transform.DOScaleY(1.2f, 0.55f));

        scaleSequence.SetLoops(-1, LoopType.Yoyo);

        scaleSequence.Play();
    }

    void CreateLoopingRotationSequence()
    {
        Sequence loopSequence = DOTween.Sequence();

        loopSequence.AppendInterval(delayBetweenLoops);

        loopSequence.Append(transform.DORotate(transform.rotation.eulerAngles + rotationAmount, rotationDuration, RotateMode.FastBeyond360)
                                       .SetEase(rotationEase));

        loopSequence.SetLoops(-1, LoopType.Restart);

        loopSequence.Play();
    }
}
