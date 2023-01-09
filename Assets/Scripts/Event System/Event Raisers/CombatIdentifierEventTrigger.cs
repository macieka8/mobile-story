using UnityEngine;

namespace Game
{
    public class CombatIdentifierEventTrigger : MonoBehaviour
    {
        [SerializeField] CombatEntityIdentifier _entityToFind;
        [SerializeField] VoidGameEvent _eventToTrigger;

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<CombatEntity>(out var foundCombatEntity))
            {
                if (foundCombatEntity.Identifier == _entityToFind)
                {
                    _eventToTrigger.RaiseEvent();
                }
            }
        }
    }
}
