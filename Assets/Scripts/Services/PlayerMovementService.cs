using System;
using Infrastructure.Factory;
using UnityEngine;
using Zenject;

namespace Services
{
    public class PlayerMovementService
    {
        private UIFactory _uiFactory;
        
        public event Action<Vector3> OnMove;

        [Inject]
        private void Inject(UIFactory uiFactory) => 
            _uiFactory = uiFactory;

        public void ProcessMovement(float speed)
        {
            Vector3 direction = new Vector3(_uiFactory.Joystick.Horizontal, 0f, _uiFactory.Joystick.Vertical).normalized;

            if (direction.magnitude > 0)
            {
                Vector3 movement = direction * speed * Time.deltaTime;
                OnMove?.Invoke(movement);
            }
        }
    }
}