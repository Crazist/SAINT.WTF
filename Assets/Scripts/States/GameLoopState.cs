using Infrastructure;
using Infrastructure.Factory;
using Interfaces;
using Zenject;

namespace States
{
    public class GameLoopState : IState
    {
        private const string gameLoopScene = "GameLoopScene";
       
        private SceneLoader _sceneLoader;
        private UIFactory _uiFactory;
        private GameFactory _gameFactory;

        [Inject]
        private void Inject(SceneLoader sceneLoader, UIFactory uiFactory, GameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _uiFactory = uiFactory;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            _sceneLoader.Load(gameLoopScene, OnLoadLevel);
        }

        public void Exit()
        {
        }

        private void OnLoadLevel()
        {
            _uiFactory.CreateUiRoot();
            _uiFactory.CreateMessageText();
            _uiFactory.CreateJoystick();
            _gameFactory.CreatePlayer();
            _gameFactory.CreateBuildings();
        }
    }
}