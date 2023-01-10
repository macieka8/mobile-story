using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Items/Buff Item")]
    public class BuffItem : ActivableItem
    {
        [SerializeField] BuffData _buff;

        public override void Activate(GameObject target)
        {
            if (target.TryGetComponent<BuffableEntity>(out var buffableEntity))
            {
                if (target.TryGetComponent<Inventory>(out var inventory))
                {
                    if (inventory.IsItemOnCooldown(this)) return;
                    // item not found
                    if (!inventory.RemoveSingle(this)) return;
                    if (!inventory.Contains(this))
                    {
                        InvokeOnActivableRemove();
                    }
                }
                inventory.SetItemOnCooldown(this);
                buffableEntity.AddBuff(_buff.InitializeBuff(target));
                InvokeOnActivate();
            }
        }
    }
}
