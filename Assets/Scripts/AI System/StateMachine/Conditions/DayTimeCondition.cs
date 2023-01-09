using UnityEngine;

namespace Game
{
    public class DayTimeCondition : StateTransitionCondition
    {
        [SerializeField] int _startHour;
        [SerializeField] int _endHour;

        public override bool IsMet()
        {
            var currenthour = TimeSystem.Instance.Hour;
            return currenthour >= _startHour && currenthour < _endHour;
        }
    }
}
