using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class ItemSlotUI : MonoBehaviour, IDropHandler
    {
        [SerializeField] ItemUI _item;

        Inventory _inventory;
        int _slotIndex;

        void OnDestroy()
        {
            if (_inventory != null)
            {
                _inventory.OnInventoryChanged -= HandleInventoryUpdate;
            }
        }

        void HandleInventoryUpdate()
        {
            _item.UpdateUI(_inventory.InventorySlots[_slotIndex]);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag.TryGetComponent<ItemUI>(out var itemUi))
            {
                _inventory.Swap(itemUi.SlotIndex, _slotIndex);
            }
        }

        public void Setup(Inventory inventory, int slotIndex, Transform parentWhenDragged)
        {
            _inventory = inventory;
            _inventory.OnInventoryChanged += HandleInventoryUpdate;

            _slotIndex = slotIndex;
            _item.Setup(_slotIndex, parentWhenDragged, inventory);

            HandleInventoryUpdate();
        }
    }
}