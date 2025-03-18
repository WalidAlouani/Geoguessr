using Zenject;

public class ProjectInstaller : MonoInstaller<ProjectInstaller>
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<QuizFinishedSignal>();
        Container.Bind<SceneLoader>().FromInstance(SceneLoader.Instance).AsSingle();
    }
}
