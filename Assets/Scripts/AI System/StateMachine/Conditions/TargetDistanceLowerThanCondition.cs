using UnityEngine;

namespace Game
{
    public class TargetDistanceLowerThanCondition : StateTransitionCondition
    {
        [SerializeField] float _distance;
        ITargetingHelper _targetingHelper;

        float _distanceSqr;

        void Awake()
        {
            _distanceSqr = _distance * _distance;
            _targetingHelper = GetComponentInParent<ITargetingHelper>();
        }

        public override bool IsMet()
        {
            return (_targetingHelper.Target == null)
                ? false
                : (_targetingHelper.Target.Position - (Vector2)transform.position).sqrMagnitude < _distanceSqr;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _distance);
        }
    }
}
