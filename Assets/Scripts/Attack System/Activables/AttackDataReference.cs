using UnityEngine;

namespace Game
{
    public class AttackDataReference : ActivableReference
    {
        public AttackData _attackData;

        public override IActivable GetActivable()
        {
            return _attackData;
        }
    }
}
