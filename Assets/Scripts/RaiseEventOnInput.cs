using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class RaiseEventOnInput : MonoBehaviour
    {
        [SerializeField] InputActionReference _inputAction;
        [SerializeField] VoidGameEvent _gameEvent;

        void OnEnable()
        {
            _inputAction.action.performed += HandleInput;
            _inputAction.action.Enable();
        }

        void OnDisable()
        {
            _inputAction.action.performed -= HandleInput;
        }

        void HandleInput(InputAction.CallbackContext obj)
        {
            _gameEvent.RaiseEvent();
        }
    }
}
