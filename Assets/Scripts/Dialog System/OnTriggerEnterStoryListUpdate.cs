using UnityEngine;

namespace Game
{
    public class OnTriggerEnterStoryListUpdate : MonoBehaviour
    {
        [SerializeField] GameStory _story;

        [SerializeField] CombatEntityIdentifier _entityIdentifier;
        [SerializeField] string _listName;
        [SerializeField] string _valueToAdd;

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<CombatEntity>(out var combatEntity))
            {
                if (combatEntity.Identifier == _entityIdentifier)
                {
                    _story.AddListElement(_listName, _valueToAdd);
                }
            }
        }
    }
}

