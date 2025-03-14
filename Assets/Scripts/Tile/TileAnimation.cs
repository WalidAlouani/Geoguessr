using DG.Tweening;
using UnityEngine;

public class TileAnimation : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Animator _animator;

    private int StopOnHash = Animator.StringToHash("StepOn");

    public void PlayStartAnimation(int index)
    {
        var initialScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(initialScale, 0.5f).SetDelay(index * 0.01f).SetEase(Ease.InOutCubic);
    }

    public void PlayStepOnAnimation()
    {
        PlayEmission();
        PlayPositionMovement();
    }

    private void PlayEmission()
    {
        var material = _renderer.material;

        Color originalEmission = material.GetColor("_EmissionColor");

        material.EnableKeyword("_EMISSION");

        Sequence seq = DOTween.Sequence();
        seq.Append(material.DOColor(Color.white, "_EmissionColor", 0.15f))
           .Append(material.DOColor(originalEmission, "_EmissionColor", 0.15f));
    }

    private void PlayPositionMovement()
    {
        _animator.SetTrigger(StopOnHash);
    }
}
