using UnityEngine;

namespace Game
{
    public class OnTriggerChangeSoundtrack : MonoBehaviour
    {
        [SerializeField] CombatEntityIdentifier _entityIdentifier;
        [SerializeField] SoundtrackPlayer _soundtrackPlayer;
        [SerializeField] Soundtrack _newSoundtrack;

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<CombatEntity>(out var foundEntity)
                && foundEntity.Identifier == _entityIdentifier)
            {
                _soundtrackPlayer.PlaySoundtrack(_newSoundtrack);
            }
        }
    }
}
