using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class InteractableMessageDisplayer : MonoBehaviour
    {
        [SerializeField] GameObject _interactMessage;
        [SerializeField] Button _interactButton;

        IInteractable _interactable;

        void Awake()
        {
            _interactable = GetComponent<IInteractable>();
        }

        void Start()
        {
            _interactButton.onClick.AddListener(() => { _interactable.Interact(); });
            HideInteractMessage();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                ShowInteractMessage();
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                HideInteractMessage();
            }
        }

        void ShowInteractMessage()
        {
            _interactMessage.SetActive(true);
        }

        void HideInteractMessage()
        {
            _interactMessage.SetActive(false);
        }
    }
}
