using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Game
{
    public class ItemDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] Transform _parentWhenDragged;
        CanvasGroup _canvasGroup;

        bool _isHovering = false;
        Transform _originalParent;

        public Transform ParentWhenDragged { get => _parentWhenDragged; set => _parentWhenDragged = value; }
        public bool IsHovering => _isHovering;

        void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            _originalParent = transform.parent;
            transform.SetParent(_parentWhenDragged);
            _canvasGroup.blocksRaycasts = false;
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            transform.position = Pointer.current.position.ReadValue();
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            transform.SetParent(_originalParent);
            transform.localPosition = Vector3.zero;
            _canvasGroup.blocksRaycasts = true;
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            _isHovering = true;
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            _isHovering = false;
        }
    }
}
