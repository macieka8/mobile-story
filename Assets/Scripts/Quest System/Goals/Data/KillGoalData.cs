using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Quest/Goal/KillGoal")]
    public class KillGoalData : GoalData
    {
        [SerializeField] int _requiredKillCount;
        [SerializeField] CombatEntityIdentifier _entityType;

        public int RequiredKillCount => _requiredKillCount;
        public CombatEntityIdentifier CombatEntityType => _entityType;

        public override Goal InitializeGoal()
        {
            return new KillGoal(this);
        }
    }
}
