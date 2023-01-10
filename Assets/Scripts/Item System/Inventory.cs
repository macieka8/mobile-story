using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class Inventory : MonoBehaviour, IPersistant
    {
        class FloatCooldown
        {
            public float Value;
            public FloatCooldown(float value) => Value = value;
        }

        struct PersistantData
        {
            public List<(string, int)> ItemSlots;
        }

        [SerializeField] AssetLabelReference _itemLabel;
        [SerializeField] int _maxItemCount;
        [SerializeField] List<ItemSlot> _startingItems;
        
        List<ItemSlot> _inventorySlots;
        Dictionary<ActivableItem, FloatCooldown> _itemsCooldowns = new Dictionary<ActivableItem, FloatCooldown>();

        public List<ItemSlot> InventorySlots => _inventorySlots;

        public event Action OnInventoryChanged = delegate { };
        public event Action<ItemSlot> OnItemAdd = delegate { };

        void Awake()
        {
            _inventorySlots = new List<ItemSlot>(_maxItemCount);
            for (int i = 0; i < _maxItemCount; i++)
            {
                if (i < _startingItems.Count)
                    _inventorySlots.Add(_startingItems[i]);
                else
                    _inventorySlots.Add(new ItemSlot());
            }
        }

        void OnEnable() => OnInventoryChanged?.Invoke();
        void Start() => OnInventoryChanged?.Invoke();

        void Update()
        {
            foreach (var entry in _itemsCooldowns)
            {
                if (entry.Value.Value > 0f)
                    _itemsCooldowns[entry.Key].Value -= Time.deltaTime;
            }
        }

        public bool IsItemOnCooldown(ActivableItem item)
        {
            if (_itemsCooldowns.TryGetValue(item, out var cooldown))
            {
                return cooldown.Value > 0f;
            }
            return false;
        }

        public void SetItemOnCooldown(ActivableItem item)
        {
            if (_itemsCooldowns.TryGetValue(item, out var cooldown))
            {
                _itemsCooldowns[item].Value = item.CooldownInSeconds;
            }
            else
            {
                _itemsCooldowns.Add(item, new FloatCooldown(item.CooldownInSeconds));
            }
        }

        public void RemoveAt(int index)
        {
            if (index >= _inventorySlots.Count || index < 0) return;
            _inventorySlots[index] = new ItemSlot();
            OnInventoryChanged?.Invoke();
        }

        public bool RemoveSingle(Item item)
        {
            for (int i = 0; i < _inventorySlots.Count; i++)
            {
                var currentItemSlot = _inventorySlots[i];
                if (currentItemSlot.Item == item)
                {
                    currentItemSlot.Count--;
                    if (currentItemSlot.Count == 0)
                    {
                        _inventorySlots[i] = new ItemSlot();
                    }
                    else
                    {
                        _inventorySlots[i] = currentItemSlot;
                    }
                    OnInventoryChanged?.Invoke();
                    return true;
                }
            }
            return false;
        }

        public bool Contains(Item item)
        {
            for (int i = 0; i < _inventorySlots.Count; i++)
            {
                if (_inventorySlots[i].Item == item)
                    return true;
            }
            return false;
        }

        public void AddItem(ItemSlot itemSlotToAdd)
        {
            for (int i = 0; i < _inventorySlots.Count; i++)
            {
                if (itemSlotToAdd.Item != _inventorySlots[i].Item) continue;

                var currentSlot = _inventorySlots[i];
                var availableSlotCount = itemSlotToAdd.Item.MaxStackCount - currentSlot.Count;
                // Can fit into slot
                if (availableSlotCount >= itemSlotToAdd.Count)
                {
                    currentSlot.Count += itemSlotToAdd.Count;

                    _inventorySlots[i] = currentSlot;
                    OnInventoryChanged?.Invoke();
                    OnItemAdd?.Invoke(itemSlotToAdd);
                    return;
                }
                // Not enought space
                else
                {
                    currentSlot.Count += availableSlotCount;
                    itemSlotToAdd.Count -= availableSlotCount;

                    _inventorySlots[i] = currentSlot;
                }
            }

            // Look for empty slot
            for (int i = 0; i < _inventorySlots.Count; i++)
            {
                if (_inventorySlots[i].Item != null) continue;
                _inventorySlots[i] = itemSlotToAdd;
                OnInventoryChanged?.Invoke();
                OnItemAdd?.Invoke(itemSlotToAdd);
                return;
            }
        }

        public void Swap(int indexA, int indexB)
        {
            if (indexA >= _inventorySlots.Count || indexA < 0) return;
            if (indexB >= _inventorySlots.Count || indexB < 0) return;

            var itemA = _inventorySlots[indexA];
            _inventorySlots[indexA] = _inventorySlots[indexB];
            _inventorySlots[indexB] = itemA;
            OnInventoryChanged?.Invoke();
        }

        public object Save()
        {
            var items = new List<(string, int)>();
            foreach (var itemSlot in _inventorySlots)
            {
                if (itemSlot.Item == null)
                    items.Add(("", 0));
                else
                   items.Add((itemSlot.Item.name, itemSlot.Count));
            }

            return new PersistantData
            {
                ItemSlots = items
            };
        }

        public void Load(object data, IGameDataHandler dataHandler)
        {
            var decodedData = dataHandler.ToObject<PersistantData>(data);

            // Reset Inventory Slots
            _itemsCooldowns = new Dictionary<ActivableItem, FloatCooldown>();
            _inventorySlots = new List<ItemSlot>();
            for (int i = 0; i < _maxItemCount; i++)
                _inventorySlots.Add(new ItemSlot());

            // Add All Items
            var loadItemsHandler = Addressables.LoadAssetsAsync<Item>(_itemLabel, (item) => {
                var foundIndex = decodedData.ItemSlots.FindIndex((decodedItem) => decodedItem.Item1 == item.name);
                if (foundIndex != -1)
                {
                    _inventorySlots[foundIndex] = new ItemSlot(item, decodedData.ItemSlots[foundIndex].Item2);
                }
            });

            // Notify when completed
            loadItemsHandler.Completed += (AsyncOperationHandle<IList<Item>> asyncOp) => OnInventoryChanged?.Invoke();
        }

        public Type GetPersistantDataType()
        {
            return typeof(PersistantData);
        }
    }

}
