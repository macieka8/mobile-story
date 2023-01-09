using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Game
{
    public class NewSpellAnnouncerUI : MonoBehaviour
    {
        [SerializeField] Spellbook _spellbook;
        [SerializeField] TextMeshProUGUI _spellNameText;
        [SerializeField] Image _spellImage;
        [SerializeField] Animator _animator;

        void OnEnable()
        {
            _spellbook.onSpellAdd += HandleSpellAdded;
        }

        void OnDisable()
        {
            _spellbook.onSpellAdd -= HandleSpellAdded;
        }

        void HandleSpellAdded(AttackData spell)
        {
            _spellNameText.text = spell.name;
            _spellImage.sprite = spell.Icon;
            _animator.Play("AnnounceNewSpellAdded");
        }
    }
}
