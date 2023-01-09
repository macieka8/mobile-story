using UnityEngine;

namespace Game
{
    public class CreditRolls : MonoBehaviour
    {
        [SerializeField] float _rollDurationInSeconds;
        [SerializeField] float _waitAfterDisplayedDurationInSeconds = 5f;
        [SerializeField] VoidGameEvent _onCreditRollFinishedEvent;

        RectTransform _rectTransform;

        Vector3 _startPosition;
        Vector3 _endPosiiton;
        float _currentTime;

        void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        void Update()
        {
            _currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(_startPosition, _endPosiiton, _currentTime / _rollDurationInSeconds);
            if (_currentTime >= _rollDurationInSeconds + _waitAfterDisplayedDurationInSeconds)
            {
                _onCreditRollFinishedEvent.RaiseEvent();
                enabled = false;
            }
        }

        void OnEnable()
        {
            RollCredits();
        }

        void RollCredits()
        {
            _currentTime = 0f;
            _startPosition = new Vector3(transform.position.x, -_rectTransform.rect.height / 2f, transform.position.z);
            _endPosiiton = new Vector3(transform.position.x, _rectTransform.rect.height / 2f, transform.position.z);
        }
    }
}
