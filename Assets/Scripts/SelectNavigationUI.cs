using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class SelectNavigationUI : MonoBehaviour
    {
        [SerializeField] GameObject _defaultSelectedGameObject;

        void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(_defaultSelectedGameObject);
        }
    }
}
