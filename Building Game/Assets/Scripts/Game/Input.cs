using UnityEngine;
using UnityEngine.InputSystem;
using Game.BuildingComponents;

namespace Game
{
    public class Input : MonoBehaviour
    {
        [SerializeField] private Builder _builder;
        [SerializeField] private Camera _camera;

        private PlayerInputActions _playerInput;
        private InputAction _getPosition;
        private InputAction _interact;

        public void Initialize()
        {
            _playerInput = new PlayerInputActions();
            _getPosition = _playerInput.Player.GetPosition;
            _interact = _playerInput.Player.Interact;
        }

        private void OnEnable()
        {
            _getPosition.Enable();
            _interact.Enable();
        }

        private void OnDisable()
        {
            _getPosition.Disable();
            _interact.Disable();
        }

        private void Update()
        {
            var position = _camera.ScreenToWorldPoint(_getPosition.ReadValue<Vector2>());
            _builder.UpdatePosition(position);
            if (_interact.triggered) _builder.TryInteract(position);
        }
    }
}