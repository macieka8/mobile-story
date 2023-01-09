using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

namespace Game
{
    public class SpellUI : ItemDragHandler, IActivableUI
    {
        [SerializeField] Image _spellIcon;

        [SerializeField] GameObject _onHoverInfo;
        [SerializeField] TextMeshProUGUI _onHoverText;

        AttackData _spell;

        public IActivable Activable => _spell;

        void Update()
        {
            if (IsHovering)
            {
                _onHoverInfo.transform.position = Mouse.current.position.ReadValue();
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            if (IsHovering)
            {
                _onHoverInfo.transform.position = Mouse.current.position.ReadValue();
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (_spell != null)
                _onHoverInfo.SetActive(true);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            _onHoverInfo.SetActive(false);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
        }

        public void SetSpell(AttackData spell)
        {
            _spell = spell;
            if (_spell != null)
            {
                _onHoverText.text = _spell.Description;
            }
            else
            {
                _onHoverText.text = "";
            }
            _spellIcon.sprite = spell.Icon;
        }
    }
}
