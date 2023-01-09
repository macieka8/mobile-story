using UnityEngine;

namespace Game
{
    public class DoorOpener : MonoBehaviour
    {
        [SerializeField] LayerMask _interactableLayer;
        Door _mostRecentDoor;

        void Update()
        {
            if (_mostRecentDoor != null && Vector2.Distance(_mostRecentDoor.transform.position, transform.position) > 1f)
            {
                if (_mostRecentDoor.IsOpen)
                    _mostRecentDoor.Interact();

                _mostRecentDoor = null;
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            var interactable = Physics2D.OverlapPoint(collision.collider.transform.position, _interactableLayer);
            if (interactable)
            {
                if (interactable.TryGetComponent<Door>(out var door))
                {
                    if (!door.IsOpen)
                    {
                        door.Interact();
                        _mostRecentDoor = door;
                    }
                }
            }
        }
    }
}
