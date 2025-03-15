using Zenject;

public class GameInstaller : MonoInstaller<GameInstaller>
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        // Declare your signals.
        Container.DeclareSignal<RollDiceSignal>();
        Container.DeclareSignal<DiceRolledSignal>();

        // Bind your game-specific services.
        //Container.Bind<IDiceService>().To<DiceService>().AsSingle();
        Container.BindInterfacesAndSelfTo<TurnManager>().AsSingle();
        Container.BindFactory<PlayerControllerAI, PlayerControllerAIFactory>()
                   .FromComponentInNewPrefabResource("Prefabs/PlayerControllerAI")
                   .UnderTransformGroup("AIPlayers");
    }
}
