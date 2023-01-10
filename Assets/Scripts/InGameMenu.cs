using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class InGameMenu : MonoBehaviour
    {
        [SerializeField] InputActionReference _triggerMenuAction;
        [SerializeField] InputActionReference _triggerSpellbookAction;
        [SerializeField] GameObject _menu;
        [SerializeField] GameObject _spellbook;

        [SerializeField] VoidGameEvent _spellbookOpenedGameEvent;
        [SerializeField] VoidGameEvent _spellbookClosedGameEvent;

        void Awake()
        {
            _menu.SetActive(false);
            _spellbook.SetActive(false);
        }

        void OnEnable()
        {
            _triggerMenuAction.action.performed += HandleMenuOnOff;
            _triggerSpellbookAction.action.performed += HandleSpellbookOnOff;
        }

        void OnDisable()
        {
            _triggerMenuAction.action.performed -= HandleMenuOnOff;
            _triggerSpellbookAction.action.performed -= HandleSpellbookOnOff;
        }
        void HandleMenuOnOff(InputAction.CallbackContext obj)
        {
            ToggleMenu();
        }

        public void ToggleMenu()
        {
            if (_spellbook.activeInHierarchy)
            {
                _spellbookClosedGameEvent.RaiseEvent();
                _spellbook.SetActive(false);
                TimeSystem.Instance.SetPause(false);
            }
            else
            {
                _menu.SetActive(!_menu.activeInHierarchy);
                TimeSystem.Instance.SetPause(_menu.activeInHierarchy);
            }
        }

        void HandleSpellbookOnOff(InputAction.CallbackContext obj)
        {
            _spellbook.SetActive(!_spellbook.activeInHierarchy);

            if (_spellbook.activeInHierarchy) _spellbookOpenedGameEvent.RaiseEvent();
            else _spellbookClosedGameEvent.RaiseEvent();
        }
    }
}
