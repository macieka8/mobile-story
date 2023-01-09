using UnityEngine;

namespace Game
{
    public class DistanceCondition : StateTransitionCondition
    {
        [SerializeField] Transform objectOne;
        [SerializeField] Transform objectTwo;
        [SerializeField] float _minimalDistanceToSatisfyCondition;

        float _minDistanceSquared;

        void Awake()
        {
            _minDistanceSquared = _minimalDistanceToSatisfyCondition * _minimalDistanceToSatisfyCondition;
        }

        public override bool IsMet()
        {
            var distanceSqr = ((Vector2)(objectOne.position - objectTwo.position)).sqrMagnitude;
            return _minDistanceSquared <= distanceSqr;
        }
    }
}
