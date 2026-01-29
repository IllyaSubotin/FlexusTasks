using UnityEngine;
using Zenject;

public class MenuInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<StateMachine>().AsSingle();

        Container.Bind<MenuScreen>().FromComponentInHierarchy().AsSingle();
        
        Container.Bind<State>().To<MenuState>().AsSingle();

        Container.Bind<MenuBootstrap>().FromComponentInHierarchy().AsSingle();
    }
}
