using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class QuizInstaller : MonoInstaller<GameInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IAssetLoader>().To<AddressableLoader>().AsSingle();
    }
}