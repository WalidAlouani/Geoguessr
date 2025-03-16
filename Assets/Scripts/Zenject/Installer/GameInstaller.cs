using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller<GameInstaller>
{
    [SerializeField] private PlayerPrefabMapping playerPrefabMapping;
    [SerializeField] private TilePrefabMapping tilePrefabMapping;
    
    public override void InstallBindings()
    {
        Container.DeclareSignal<PlayersCreatedSignal>();

        Container.DeclareSignal<RollDiceSignal>();
        Container.DeclareSignal<DiceRolledSignal>();

        Container.DeclareSignal<TurnStartedSignal>();
        Container.DeclareSignal<TurnEndedSignal>();

        Container.DeclareSignal<PlayerStartMoveSignal>();

        Container.DeclareSignal<TileReachedSignal>();
        Container.DeclareSignal<TileStoppedSignal>();

        Container.DeclareSignal<CoinsUpdateSignal>();

        Container.DeclareSignal<QuizRequestedSignal>();
        
        Container.Bind<IPlayerFactory>().To<CompositePlayerFactory>().AsSingle();
        Container.BindInstance(playerPrefabMapping).IfNotBound();

        Container.Bind<ITileFactory>().To<CompositeTileFactory>().AsSingle();
        Container.BindInstance(tilePrefabMapping).IfNotBound();

        Container.BindInterfacesAndSelfTo<TurnManager>().AsSingle();
        Container.Bind<CommandQueue>().AsSingle();
    }
}
