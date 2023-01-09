using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    [CreateAssetMenu(menuName = "Combat/Fraction")]
    public class CombatFraction : ScriptableObject
    {
        [SerializeField] List<CombatFraction> _allyFractions;

        public string Name => name;
        public IReadOnlyList<CombatFraction> AllyFractions => _allyFractions;

        public bool IsAllied(CombatFraction fraction)
        {
            return _allyFractions.Contains(fraction);
        }
    }
}
