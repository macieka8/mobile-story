using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ReviveOnEntityDead : MonoBehaviour
    {
        [SerializeField] float _timeToReviveInSeconds;

        IHealthEntity _healthEntity;
        WaitForSeconds _waitForRevive;

        void Awake()
        {
            _healthEntity = GetComponent<IHealthEntity>();
            _waitForRevive = new WaitForSeconds(_timeToReviveInSeconds);
        }

        void OnEnable()
        {
            _healthEntity.OnEntityDead += HandleEntityDead;
        }

        void OnDisable()
        {
            _healthEntity.OnEntityDead -= HandleEntityDead;
        }

        void HandleEntityDead()
        {
            StartCoroutine(ReviveCoroutine());
        }

        IEnumerator ReviveCoroutine()
        {
            yield return _waitForRevive;
            while (CombatSystem.Instance.IsPlayerInCombat) yield return null;

            _healthEntity.ModifyHealth(_healthEntity.MaxHealth.Value / 2);
        }
    }
}
