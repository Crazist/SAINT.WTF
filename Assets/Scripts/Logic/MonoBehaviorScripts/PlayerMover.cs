using Services;
using UnityEngine;
using Zenject;

namespace Logic.MonoBehaviorScripts
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _rotationSpeed = 10f;

        private PlayerMovementService _playerMovementService;
        private CharacterController _characterController;
        private Vector3 _targetPosition;

        [Inject]
        private void Inject(PlayerMovementService playerMovementService) => 
            _playerMovementService = playerMovementService;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _targetPosition = transform.position;
            _playerMovementService.OnMove += MovePlayer;
        }

        private void OnDestroy() => _playerMovementService.OnMove -= MovePlayer;

        private void Update()
        {
            _playerMovementService.ProcessMovement(_moveSpeed);
            MoveCharacter();
        }

        private void MoveCharacter()
        {
            Vector3 currentPosition = transform.position;

            Vector3 interpolatedPosition = Vector3.Lerp(currentPosition, _targetPosition, 0.1f);

            Vector3 movement = interpolatedPosition - currentPosition;

            CollisionFlags flags = _characterController.Move(movement);

            if (movement != Vector3.zero)
            {
                RotateTowards(movement);
            }

            if ((flags & CollisionFlags.Sides) != 0)
            {
                _targetPosition = transform.position;
            }
        }

        private void MovePlayer(Vector3 movement) => _targetPosition += movement;

        private void RotateTowards(Vector3 direction)
        {
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
            }
        }

    }
}