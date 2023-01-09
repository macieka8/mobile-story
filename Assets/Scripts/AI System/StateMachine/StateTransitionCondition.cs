using UnityEngine;

namespace Game
{
    public abstract class StateTransitionCondition : MonoBehaviour
    {
        public abstract bool IsMet();
    }
}
