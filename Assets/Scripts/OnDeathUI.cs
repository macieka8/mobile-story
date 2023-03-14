using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class OnDeathUI : MonoBehaviour
    {
        [SerializeField] PersistantDataManager _persistantDataManager;

        public void OnDeath()
        {
            if (!_persistantDataManager.TryLoad())
            {
                SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
            }
        }
    }
}
