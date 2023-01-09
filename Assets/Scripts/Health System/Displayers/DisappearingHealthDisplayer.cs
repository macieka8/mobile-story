using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Game
{
    public class DisappearingHealthDisplayer : MonoBehaviour
    {
        [SerializeField] BaseHealthEntity _healthEntity;
        [SerializeField] float _visibilityDurationInSeconds;
        [SerializeField] GameObject _visibleParts;
        Slider _slider;

        bool _isVisible = true;
        float _currentTimeToHideInSeconds;

        void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        void Start()
        {
            HideDisplayer();
        }

        void OnEnable()
        {
            _healthEntity.OnHealthChanged += HandleHealthChanged;
            _healthEntity.MaxHealth.OnValueChanged += HandleHealthChanged;
        }

        void OnDisable()
        {
            _healthEntity.OnHealthChanged -= HandleHealthChanged;
            _healthEntity.MaxHealth.OnValueChanged -= HandleHealthChanged;
        }

        void HideDisplayer()
        {
            if (!_isVisible) return;
            _visibleParts.SetActive(false);
            _isVisible = false;
        }

        void ShowDisplayer()
        {
            _currentTimeToHideInSeconds = _visibilityDurationInSeconds;

            if (_isVisible) return;
            _visibleParts.SetActive(true);
            StartCoroutine(WaitToDisappear());
        }

        void HandleHealthChanged()
        {
            ShowDisplayer();
            _slider.value = _healthEntity.Health / _healthEntity.MaxHealth.Value;
        }

        IEnumerator WaitToDisappear()
        {
            _isVisible = true;
            while (_currentTimeToHideInSeconds > 0f)
            {
                _currentTimeToHideInSeconds -= Time.deltaTime;
                yield return null;
            }

            HideDisplayer();
        }
    }
}
