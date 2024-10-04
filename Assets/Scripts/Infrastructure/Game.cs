using StateMachine;
using Zenject;

namespace Infrastructure
{
    public class Game
    {
        public GameStateMachine StateMachine;

        [Inject]
        private void Inject(GameStateMachine gameStateMachine) => 
            StateMachine = gameStateMachine;
    }
}