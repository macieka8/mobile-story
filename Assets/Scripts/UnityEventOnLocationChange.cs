using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class UnityEventOnLocationChange : MonoBehaviour
    {
        [SerializeField] bool _triggerOnlyOnce = false;
        [SerializeField] CombatEntityIdentifier _triggerIdentifier;
        [SerializeField] UnityEvent _onEnter;
        [SerializeField] UnityEvent _onExit;

        bool _entered = false;
        bool _exited = false;

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (_entered) return;
            if (collision.TryGetComponent<CombatEntity>(out var foundCombatEntity)
                && foundCombatEntity.Identifier == _triggerIdentifier)
            {
                if (_triggerOnlyOnce) _entered = true;
                _onEnter.Invoke();
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (_exited) return;
            if (collision.TryGetComponent<CombatEntity>(out var foundCombatEntity)
                && foundCombatEntity.Identifier == _triggerIdentifier)
            {
                if (_triggerOnlyOnce) _exited = true;
                _onExit.Invoke();
            }
        }
    }
}
