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

        Container.DeclareSignal<CoinsAddedSignal>();
        Container.DeclareSignal<CoinsUpdateSignal>();

        Container.DeclareSignal<QuizRequestedSignal>();

        Container.Bind<PlayersManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<TurnManager>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<BoardManager>().AsSingle().NonLazy();

        Container.Bind<IPlayerFactory>().To<CompositePlayerFactory>().AsSingle();
        Container.BindInstance(playerPrefabMapping).IfNotBound();

        Container.Bind<ITileFactory>().To<CompositeTileFactory>().AsSingle();
        Container.BindInstance(tilePrefabMapping).IfNotBound();

        Container.Bind<CommandQueue>().AsSingle();

        Container.Bind<IAssetLoader>().To<AddressableLoader>().AsSingle();
    }
}
