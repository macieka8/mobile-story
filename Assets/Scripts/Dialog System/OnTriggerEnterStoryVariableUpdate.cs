using UnityEngine;

namespace Game
{
    public class OnTriggerEnterStoryVariableUpdate : MonoBehaviour
    {
        [SerializeField] GameStory _story;

        [SerializeField] CombatEntityIdentifier _entityIdentifier;
        [SerializeField] string _variableName;
        [SerializeField] string _newValue;

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<CombatEntity>(out var combatEntity))
            {
                if (combatEntity.Identifier == _entityIdentifier)
                {
                    _story.SetVariable(_variableName, _newValue);
                }
            }
        }
    }
}

