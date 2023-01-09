using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Combat/Identifer")]
    public class CombatEntityIdentifier : ScriptableObject
    {
        [SerializeField] string _name;
        [SerializeField] CombatFraction _fraction;

        public string Name => _name;
        public CombatFraction Fraction=> _fraction;

        public void ChangeFraction(CombatFraction fraction)
        {
            _fraction = fraction;
        }
    }
}
