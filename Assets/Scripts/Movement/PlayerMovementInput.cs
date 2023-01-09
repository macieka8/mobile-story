using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace Game
{
    public class PlayerMovementInput : MonoBehaviour
    {
        [SerializeField] InputActionReference _movementInput;
        AttackingEntity _attackingEntity;
        IMovementController _movementController;

        void Awake()
        {
            _attackingEntity = GetComponent<AttackingEntity>();
            _movementController = GetComponent<IMovementController>();
        }

        void OnEnable()
        {
            _movementInput.action.performed += HandleMovementInput;
            _movementInput.action.canceled += HandleMovementInput;
            _movementInput.action.Enable();
        }

        void OnDisable()
        {
            _movementInput.action.performed -= HandleMovementInput;
            _movementInput.action.canceled -= HandleMovementInput;
            _movementInput.action.Disable();
        }

        void HandleMovementInput(InputAction.CallbackContext obj)
        {
            var input = obj.ReadValue<Vector2>();
            if (input != Vector2.zero) _attackingEntity.AttackDirection = input;
            _movementController.SetMovementInput(input);
        }
    }
}
