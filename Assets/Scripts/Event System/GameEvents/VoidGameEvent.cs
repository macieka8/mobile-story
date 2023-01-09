using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Event/Void Event")]
    public class VoidGameEvent : BaseGameEvent<Void>
    {
        public void RaiseEvent() => RaiseEvent(new Void { });
    }
}
