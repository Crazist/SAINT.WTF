using States;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class GameBootstrapper : MonoBehaviour
    {
        private Game _game;

        [Inject]
        private void Inject(Game game) => 
            _game = game;

        private void Awake()
        {
            _game.StateMachine.InitStates();
            _game.StateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}