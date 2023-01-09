using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    public class SpellbookUI : MonoBehaviour
    {
        [SerializeField] Spellbook _spellbook;
        [SerializeField] Transform _container;
        [SerializeField] Transform _draggedObjectsParent;
        [SerializeField] GameObject _spellUiPrefab;

        void Start()
        {
            foreach (var spell in _spellbook.Spells)
            {
                AddSpellUi(spell);
            }
            _spellbook.onSpellAdd += AddSpellUi;
        }

        void OnDestroy()
        {
            _spellbook.onSpellAdd -= AddSpellUi;
        }

        void AddSpellUi(AttackData spell)
        {
            var spellUi = Instantiate(_spellUiPrefab, _container)
                .GetComponentInChildren<SpellUI>();
            spellUi.SetSpell(spell);
            spellUi.ParentWhenDragged = _draggedObjectsParent;
        }
    }
}
