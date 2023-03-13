using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Game
{
    public class ItemUI : ItemDragHandler, IActivableUI
    {
        [SerializeField] TextMeshProUGUI _countText;
        [SerializeField] Image _itemIcon;
        [SerializeField] Sprite _emptyIcon;

        [SerializeField] GameObject _onHoverInfo;
        [SerializeField] TextMeshProUGUI _onHoverText;

        int _slotIndex;
        Item _item;

        public int SlotIndex => _slotIndex;

        public IActivable Activable => _item is ActivableItem ? _item as ActivableItem : null;

        void Update()
        {
            if (IsHovering)
            {
                _onHoverInfo.transform.position = Pointer.current.position.ReadValue();
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (_item != null)
                _onHoverInfo.SetActive(true);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            _onHoverInfo.SetActive(false);
        }

        public void Setup(int slotIndex, Transform parentWhenDragged, Inventory inventory)
        {
            _item = inventory.InventorySlots[slotIndex].Item;
            _slotIndex = slotIndex;
            ParentWhenDragged = parentWhenDragged;
        }

        public void UpdateUI(ItemSlot itemSlot)
        {
            _item = itemSlot.Item;
            if (itemSlot.Item != null)
            {
                _itemIcon.sprite = itemSlot.Item.Icon;
                _countText.text = itemSlot.Count.ToString();
                _onHoverText.text = _item.Description;
            }
            else
            {
                _itemIcon.sprite = _emptyIcon;
                _countText.text = "";
                _onHoverText.text = "";
            }
        }
    }
}