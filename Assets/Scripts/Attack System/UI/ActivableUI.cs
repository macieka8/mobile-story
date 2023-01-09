using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class ActivableUI : MonoBehaviour, IDropHandler
    {
        [SerializeField] ActivablesBinder _activables;
        [SerializeField] int _activeSpellIndex = -1;
        [SerializeField] Image _image;
        [SerializeField] GameObject _button;
        [SerializeField] Button _removeActiveSpellButton;
        [SerializeField] Slider _cooldownSlider;

        [SerializeField] Sprite _emptySprite;

        float _remainingCooldown;

        void Start()
        {
            _removeActiveSpellButton.onClick.AddListener(() => {
                _activables.RemoveActivable(_activeSpellIndex);
                _image.sprite = _emptySprite;
            });

            SetActiveSpell(_activables.Activables[_activeSpellIndex]);
            HideIfNotAssigned();
        }

        void OnEnable()
        {
            _activables.OnActivableChanged += HandleActivableChanged;
        }

        void OnDisable()
        {
            _activables.OnActivableChanged -= HandleActivableChanged;
        }

        void HandleActivableChanged(int activableIndex)
        {
            if (_activeSpellIndex != activableIndex) return;

            SetActiveSpell(_activables.Activables[activableIndex]);

            //Spellbook menu is not active
            var isInSpellbookMenu = _removeActiveSpellButton.gameObject.activeInHierarchy;
            Show();
            if (!isInSpellbookMenu)
            {
                HideIfNotAssigned();
            }
        }

        void Update()
        {
            if (_activables.Activables[_activeSpellIndex] != null)
            {
                var activable = _activables.Activables[_activeSpellIndex];

                if (activable.CooldownInSeconds == 0f)
                    _cooldownSlider.value = 0f;
                else
                    _cooldownSlider.value = _remainingCooldown / activable.CooldownInSeconds;

                if (_remainingCooldown > 0f)
                    _remainingCooldown -= Time.deltaTime;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag.TryGetComponent<IActivableUI>(out var activableUi))
            {
                _activables.SetActivableSlot(_activeSpellIndex, activableUi.Activable);
            }
        }

        void HandleActivableUsed()
        {
            if (_activables.Activables[_activeSpellIndex] == null) return;
            _remainingCooldown = _activables.Activables[_activeSpellIndex].CooldownInSeconds;
        }

        void SetActiveSpell(IActivable activable)
        {
            // Clean up after previous activable
            if (_activables.Activables[_activeSpellIndex] != null && activable != null)
            {
                activable.OnActivate -= HandleActivableUsed;
            }

            // Set up new activable ui
            if (activable != null)
            {
                _remainingCooldown = 0f;
                _image.sprite = activable.Icon;
                activable.OnActivate += HandleActivableUsed;
            }
            else
            {
                _image.sprite = _emptySprite;
            }
        }

        public void Show()
        {
            _button.SetActive(true);
            _removeActiveSpellButton.gameObject.SetActive(true);
        }

        public void HideIfNotAssigned()
        {
            _removeActiveSpellButton.gameObject.SetActive(false);
            if (_activables.Activables[_activeSpellIndex] == null)
            {
                _button.SetActive(false);
            }
        }
    }
}
