using UnityEngine;

namespace Game
{
    public class OnDestroyAddItem : MonoBehaviour
    {
        [SerializeField] ItemSlot _itemSlot;
        CombatEntity _combatEntity;

        void Awake()
        {
            _combatEntity = GetComponent<CombatEntity>();
        }

        void OnDestroy()
        {
            if(_combatEntity.LastAttacker != null && 
                _combatEntity.LastAttacker.TryGetComponent<Inventory>(out var inventory))
            {
                inventory.AddItem(_itemSlot);
            }
        }
    }
}
