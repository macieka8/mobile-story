using UnityEngine;
using System.Collections;

namespace Game
{
    public class RainOfObjectsController : MonoBehaviour
    {
        [SerializeField] RainOfObjectsAttackData _attackData;
        Collider2D _collider;

        void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        void Start()
        {
            StartCoroutine(StartRainCoroutine());
        }

        IEnumerator StartRainCoroutine()
        {
            int currentObjectsCount = 0;
            var waitForNextSpawn = new WaitForSeconds(_attackData.TimeBetweenSpawns);
            while (currentObjectsCount < _attackData.ObjectsToSpawn)
            {
                currentObjectsCount++;
                var objectsPosition = GetRandomPointInBounds(_collider.bounds);
                Instantiate(_attackData.RainObjectPrefab, objectsPosition, Quaternion.identity);
                yield return waitForNextSpawn;
            }
            Destroy(gameObject);
        }

        Vector2 GetRandomPointInBounds(Bounds bounds)
        {
            var randomPoint = new Vector2();
            do
            {
                randomPoint.x = Random.Range(bounds.center.x - bounds.extents.x, bounds.center.x + bounds.extents.x);
                randomPoint.y = Random.Range(bounds.center.y - bounds.extents.y, bounds.center.y + bounds.extents.y);

            } while (!_collider.OverlapPoint(randomPoint));

            return randomPoint;
        }
    }
}
