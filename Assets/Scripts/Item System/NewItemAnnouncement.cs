using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class NewItemAnnouncement : MonoBehaviour
    {
        [SerializeField] Inventory _inventory;
        [SerializeField] Transform _displayersContainer;

        [SerializeField] ItemAnnouncementDisplayer _itemDisplayerPrefab;

        void OnEnable() => _inventory.OnItemAdd += HandleItemAdd;
        void OnDisable() => _inventory.OnItemAdd -= HandleItemAdd;

        void HandleItemAdd(ItemSlot itemSlot)
        {
            var itemDisplayer = Instantiate(_itemDisplayerPrefab, _displayersContainer);
            itemDisplayer.Setup(itemSlot);
        }
    }
}
