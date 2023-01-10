using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Items/Attack Item")]
    public class AttackItem : ActivableItem
    {
        [SerializeField] AttackData _attackData;

        public override void Activate(GameObject target)
        {
            var attackingEntity = target.GetComponent<AttackingEntity>();
            if (attackingEntity == null) return;
            if (attackingEntity.IsAttackOnCooldown(_attackData)) return;

            var inventory = target.GetComponent<Inventory>();
            
            if (inventory.IsItemOnCooldown(this)) return;
            // Item not found
            if (!inventory.RemoveSingle(this)) return;
            if (!inventory.Contains(this))
            {
                InvokeOnActivableRemove();
            }

            inventory.SetItemOnCooldown(this);
            _attackData.Activate(target);
        }
    }
}
