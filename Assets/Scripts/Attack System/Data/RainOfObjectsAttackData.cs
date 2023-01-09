using System.Collections;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "AttackData/RainOfObjects")]
    public class RainOfObjectsAttackData : AttackData
    {
        [SerializeField] int _objectsToSpawn;
        [SerializeField] float _timeBetweenSpawns;

        [SerializeField] GameObject _rainControllerPrefab;
        [SerializeField] GameObject _rainObjectPrefab;

        public int ObjectsToSpawn => _objectsToSpawn;
        public float TimeBetweenSpawns => _timeBetweenSpawns;

        public GameObject RainControllerPrefab => _rainControllerPrefab;
        public GameObject RainObjectPrefab => _rainObjectPrefab;

        public override void StartExecution(AttackingEntity owner)
        {
            owner.StartCoroutine(PerformAttack(owner));
        }

        public override void StopExecution(AttackingEntity owner)
        {
            owner.StopCoroutine(nameof(PerformAttack));
        }

        IEnumerator PerformAttack(AttackingEntity owner)
        {
            var rainController = Instantiate(_rainControllerPrefab, owner.transform);
            owner.MovementStopper.StopMovement(CastTime);
            yield return new WaitForSeconds(CastTime);
            owner.StopAttack(this);
        }
    }
}
