using UnityEngine;

namespace Game
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] Inventory _inventory;
        [SerializeField] ItemSlotUI _itemSlotPrefab;
        [SerializeField] Transform _itemsParent;
        [SerializeField] Transform _parentWhenDragged;

        void Start()
        {
            for (int i = 0; i < _inventory.InventorySlots.Count; i++)
            {
                var itemSlot = Instantiate(_itemSlotPrefab, _itemsParent);
                itemSlot.Setup(_inventory, i, _parentWhenDragged);
            }
        }
    }
}