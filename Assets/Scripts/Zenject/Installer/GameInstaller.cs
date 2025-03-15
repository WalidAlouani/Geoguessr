using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller<GameInstaller>
{
    [SerializeField] private PlayerPrefabMapping playerPrefabMapping;
    
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<RollDiceSignal>();
        Container.DeclareSignal<DiceRolledSignal>();
        Container.DeclareSignal<TurnStartedSignal>();

        Container.BindInterfacesAndSelfTo<TurnManager>().AsSingle();
        
        Container.Bind<IPlayerFactory>().To<CompositePlayerFactory>().AsSingle();
        
        Container.BindInstance(playerPrefabMapping).IfNotBound();
    }
}
