using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game
{
    public class CombatSystem : MonoBehaviour, IPersistant
    {
        struct PersistantData
        {
            public List<string> KilledEntitiesName;
            public List<int> KillCount;
        }

        static CombatSystem _instance;
        public static CombatSystem Instance => _instance;

        [SerializeField] AssetLabelReference _identifierLabel;
        [SerializeField] float _minCombatTimeInSeconds;

        List<CombatEntityIdentifier> _allCombatEntityIdentifers = new List<CombatEntityIdentifier>();
        Dictionary<CombatEntityIdentifier, int> _killedEntities = new Dictionary<CombatEntityIdentifier, int>();
        AsyncOperationHandle<IList<CombatEntityIdentifier>> _loadIdentifiersOperation;

        bool _isPlayerInCombat = false;
        float _timeToLeaveCombat;

        public event Action<CombatEntity> OnEntityKilled = delegate { };
        public event Action<CombatEntity, CombatEntity> OnEntityAttacked = delegate { };

        public bool IsPlayerInCombat => _isPlayerInCombat;

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                _loadIdentifiersOperation = Addressables.LoadAssetsAsync<CombatEntityIdentifier>(
                    _identifierLabel,
                    addressables =>
                    {
                        _allCombatEntityIdentifers.Add(addressables);
                    });
            }
        }

        void Update()
        {
            if (_isPlayerInCombat)
            {
                _timeToLeaveCombat -= Time.deltaTime;
                if (_timeToLeaveCombat <= 0f) _isPlayerInCombat = false;
            }
        }

        void OnEnable()
        {
            OnEntityAttacked += HandleOnEntityAttacked;
            OnEntityKilled += HandleEntityKilled;
        }

        void OnDisable()
        {
            OnEntityAttacked -= HandleOnEntityAttacked;
            OnEntityKilled -= HandleEntityKilled;
        }
        void HandleEntityKilled(CombatEntity killedEntity)
        {
            if (_killedEntities.ContainsKey(killedEntity.Identifier))
            {
                _killedEntities[killedEntity.Identifier]++;
            }
            else
            {
                _killedEntities.Add(killedEntity.Identifier, 1);
            }
        }

        void HandleOnEntityAttacked(CombatEntity attacked, CombatEntity attacking)
        {
            attacked.LastAttacker = attacking;
        }

        public void InvokeOnEntityKilled(CombatEntity killedEntity)
        {
            OnEntityKilled?.Invoke(killedEntity);
        }

        public void InvokeOnEntityAttacked(CombatEntity attackedEntity, CombatEntity attackingEntity)
        {
            OnEntityAttacked(attackedEntity, attackingEntity);
        }

        public int GetKillCount(CombatEntityIdentifier identifier)
        {
            if (_killedEntities.TryGetValue(identifier, out var count))
                return count;
            return 0;
        }

        public void TriggerCombat()
        {
            _timeToLeaveCombat = _minCombatTimeInSeconds;
            _isPlayerInCombat = true;
        }

        public object Save()
        {
            var data = new PersistantData()
            {
                KilledEntitiesName = new List<string>(),
                KillCount = new List<int>(),
            };

            foreach (var item in _killedEntities)
            {
                data.KilledEntitiesName.Add(item.Key.Name);
                data.KillCount.Add(item.Value);
            }

            return data;
        }

        public void Load(object data, IGameDataHandler dataHandler)
        {
            if (!_loadIdentifiersOperation.IsDone)
            {
                Debug.Log("Waiting for Identifers to load...");
                _loadIdentifiersOperation.WaitForCompletion();
            }
            
            var dataObj = dataHandler.ToObject<PersistantData>(data);
            _killedEntities = new Dictionary<CombatEntityIdentifier, int>();

            for (int i = 0; i < dataObj.KilledEntitiesName.Count; i++)
            {
                var identifier = _allCombatEntityIdentifers.Find(id => id.Name == dataObj.KilledEntitiesName[i]);
                _killedEntities.Add(
                    identifier,
                    dataObj.KillCount[i]);
            }
        }
    }
}
