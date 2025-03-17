using UnityEngine;
using Zenject;

public class BoardEffects : MonoBehaviour
{
    [SerializeField] private FloatingTextFactory _floatingTextFactory;

    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void OnEnable()
    {
        _signalBus.Subscribe<CoinsAddedSignal>(OnCoinsAdded);
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<CoinsAddedSignal>(OnCoinsAdded);
    }

    private void OnCoinsAdded(CoinsAddedSignal signal)
    {
        _floatingTextFactory.Create(signal.Position, signal.CoinAmount);
    }
}
