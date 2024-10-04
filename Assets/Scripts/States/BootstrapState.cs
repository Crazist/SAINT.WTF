using Infrastructure;
using Interfaces;
using StateMachine;
using Zenject;

namespace States
{
    public class BootstrapState : IState
    {
        private const string Initial = "InitialScene";

        private GameStateMachine _gameStateMachine;
        private SceneLoader _sceneLoader;

        [Inject]
        private void Inject(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
            _gameStateMachine = gameStateMachine;
        }
        public void Enter()
        {
            _sceneLoader.Load(Initial, EnterLoadLevel);
        }

        private void EnterLoadLevel() => 
            _gameStateMachine.Enter<GameLoopState>();

        public void Exit()
        {
        }
    }
}