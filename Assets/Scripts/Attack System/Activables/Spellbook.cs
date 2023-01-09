using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;

namespace Game
{
    public class Spellbook : MonoBehaviour, IPersistant
    {
        struct PersistantData
        {
            public List<string> SpellNames;
        }
        static readonly string SPELLBOOK_ADDSPELL_TAG = "Spellbook.Add.";

        [SerializeField] GameStory _story;
        [SerializeField] AttackingEntity _attackingEntity;
        [SerializeField] List<AttackData> _spells = new List<AttackData>();

        public System.Action<AttackData> onSpellAdd;

        public List<AttackData> Spells => _spells;

        void OnEnable()
        {
            _story.TagResolverManager.AddTagListener(SPELLBOOK_ADDSPELL_TAG, HandleStorySpellAdd);
        }

        void OnDisable()
        {
            _story.TagResolverManager.RemoveTagListener(SPELLBOOK_ADDSPELL_TAG, HandleStorySpellAdd);
        }

        void HandleStorySpellAdd(string tag)
        {
            var spellName = tag.Substring(SPELLBOOK_ADDSPELL_TAG.Length);
            var loadSpellHandle = Addressables.LoadAssetAsync<AttackData>(spellName);
            loadSpellHandle.Completed += (AsyncOperationHandle<AttackData> asyncOp) =>
            {
                AddSpell(asyncOp.Result);
            };
        }

        public void CastSpell(int index)
        {
            _attackingEntity.PerformAttack(_spells[index]);
        }

        public void CastSpell(AttackData attackData)
        {
            if (_spells.Contains(attackData))
            {
                _attackingEntity.PerformAttack(attackData);
            }
        }

        public void AddSpell(AttackData newSpell)
        {
            if (!_spells.Contains(newSpell))
            {
                _spells.Add(newSpell);
                onSpellAdd?.Invoke(newSpell);
            }
        }

        public object Save()
        {
            var names = new List<string>(_spells.Count);
            foreach (var spell in _spells)
            {
                names.Add(spell.name);
            }

            return new PersistantData { SpellNames = names };
        }

        public void Load(object data, IGameDataHandler dataHandler)
        {
            var decodedData = dataHandler.ToObject<PersistantData>(data);
            _spells = new List<AttackData>();
            foreach (var spellName in decodedData.SpellNames)
            {
                var spellLoadHandle = Addressables.LoadAssetAsync<AttackData>(spellName);
                spellLoadHandle.Completed += (AsyncOperationHandle<AttackData> asyncOp) =>
                {
                    _spells.Add(asyncOp.Result);
                };
            }
        }
    }
}
