namespace Game
{
    public class TargetDetectedCondition : StateTransitionCondition
    {
        ITargetingHelper _targetingHelper;

        void Awake()
        {
            _targetingHelper = GetComponentInParent<ITargetingHelper>();
        }

        public override bool IsMet()
        {
            return _targetingHelper.Target != null;
        }
    }
}
