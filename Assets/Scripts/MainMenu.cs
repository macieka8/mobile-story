using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] InputActionReference _backAction;

        LinkedList<GameObject> _windows = new LinkedList<GameObject>();
        [SerializeField] List<GameObject> _initalNodes;

        void Awake()
        {
            foreach (var node in _initalNodes)
            {
                _windows.AddLast(node);
            }
        }

        void OnEnable()
        {
            _backAction.action.performed += HandleBackButtonPressed;
            _backAction.action.Enable();
        }

        void OnDisable()
        {
            _backAction.action.performed -= HandleBackButtonPressed;
        }

        void HandleBackButtonPressed(InputAction.CallbackContext obj)
        {
            Rollback();
        }

        public void ChangeWindow(GameObject window)
        {
            _windows.Last.Value.SetActive(false);
            _windows.AddLast(window);
            window.SetActive(true);
        }

        public void Rollback()
        {
            if (_windows.Count > 1)
            {
                _windows.Last.Value.SetActive(false);
                _windows.RemoveLast();

                _windows.Last.Value.SetActive(true);
            }
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
