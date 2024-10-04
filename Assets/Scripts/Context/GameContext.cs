using Infrastructure;
using Infrastructure.Factory;
using Services;
using StateMachine;
using UnityEngine;
using Zenject;

namespace Context
{
    [CreateAssetMenu(fileName = "GameplayInstaller", menuName = "Installers/GameplayInstaller")]
    public class GameContext : ScriptableObjectInstaller<GameContext>
    {
        [SerializeField] private GameObject _canvasPrefab;
        [SerializeField] private CoroutineCustomRunner _coroutineCustomRunner;

        public override void InstallBindings()
        {
            Container.Bind<CoroutineCustomRunner>().FromComponentInNewPrefab(_coroutineCustomRunner).AsSingle()
                .NonLazy();

            Container.Bind<Game>().AsSingle().NonLazy();
            Container.Bind<GameStateMachine>().AsSingle().NonLazy();
            Container.Bind<GameFactory>().AsSingle().NonLazy();
            Container.Bind<SceneLoader>().AsSingle().NonLazy();
            Container.Bind<AssetProvider>().AsSingle().NonLazy();
            Container.Bind<UIFactory>().AsSingle().NonLazy();
            Container.Bind<PlayerMovementService>().AsSingle().NonLazy();
            Container.Bind<ProductionService>().AsSingle().NonLazy();
            Container.Bind<UIService>().AsSingle().NonLazy();
        }
    }
}