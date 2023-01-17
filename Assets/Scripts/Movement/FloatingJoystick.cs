using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

namespace Game
{
    public class FloatingJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] OnScreenStick _onScreenStick;
        [SerializeField] RectTransform _joystick;

        Vector2 _originalPosition;
        Vector2 _clickedPosition;

        void Awake()
        {
            _originalPosition = _joystick.anchoredPosition;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _clickedPosition = Pointer.current.position.ReadValue();
            _joystick.position = _clickedPosition;
            _onScreenStick.OnPointerDown(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _onScreenStick.OnDrag(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _joystick.anchoredPosition = _originalPosition;
            _onScreenStick.OnPointerUp(eventData);
        }
    }
}
