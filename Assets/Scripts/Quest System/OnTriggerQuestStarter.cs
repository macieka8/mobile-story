using UnityEngine;
using System.Linq;

namespace Game
{
    public class OnTriggerQuestStarter : MonoBehaviour
    {
        [SerializeField] QuestData _questToStart;
        [SerializeField] CombatEntityIdentifier _playerIdentifier;

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<CombatEntity>(out var entity))
            {
                if (entity.Identifier == _playerIdentifier)
                {
                    if (QuestManager.Instance.StartedQuests.All(item => item.QuestData != _questToStart)
                        && QuestManager.Instance.FinishedQuests.All(item => item.QuestData != _questToStart))
                    {
                        _questToStart.StartQuest();
                    }
                    Destroy(gameObject);
                }
            }
        }
    }
}
