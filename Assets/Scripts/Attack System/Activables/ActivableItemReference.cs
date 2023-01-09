using UnityEngine;

namespace Game
{
    public class ActivableItemReference : ActivableReference
    {
        public ActivableItem _item;

        public override IActivable GetActivable()
        {
            return _item;
        }
    }
}
