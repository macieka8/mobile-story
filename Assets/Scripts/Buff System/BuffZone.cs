using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BuffZone : MonoBehaviour
    {
        [SerializeField] BuffData _buff;

        void OnTriggerStay2D(Collider2D other)
        {
            if (other.transform.parent == null) return;
            if (other.gameObject.TryGetComponent<BuffableEntity>(out var buffable))
            {
                if (buffable.Contains(_buff)) return;
                buffable.AddBuff(_buff.InitializeBuff(buffable.gameObject));
            }
            else
            {
                var foundBuffable = other.gameObject.GetComponentInParent<BuffableEntity>();
                if (foundBuffable != null)
                {
                    if (foundBuffable.Contains(_buff)) return;
                    foundBuffable.AddBuff(_buff.InitializeBuff(foundBuffable.gameObject));
                }
            }
        }
    }
}
