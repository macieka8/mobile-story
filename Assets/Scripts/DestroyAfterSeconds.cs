using UnityEngine;

namespace Game
{
    public class DestroyAfterSeconds : MonoBehaviour
    {
        [SerializeField] float _timeToLive;

        void Start()
        {
            Destroy(gameObject, _timeToLive);
        }
    }
}
