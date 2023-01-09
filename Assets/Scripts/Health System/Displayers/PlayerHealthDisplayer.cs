using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayerHealthDisplayer : MonoBehaviour
    {
        [SerializeField] BaseHealthEntity _healthEntity;
        [SerializeField] Slider _damagedSlider;
        [SerializeField] float _speed;
        Animator _animator;

        Slider _healthSlider;

        void Awake()
        {
            _healthSlider = GetComponent<Slider>();
            _animator = GetComponent<Animator>();
        }

        void Start()
        {
            HandleHealthChanged();
        }

        void OnEnable()
        {
            _healthEntity.OnHealthChanged += HandleHealthChanged;
            _healthEntity.MaxHealth.OnValueChanged += HandleHealthChanged;
        }

        void Update()
        {
            if (_damagedSlider == null) return;
            if (_damagedSlider.value > _healthSlider.value)
            {
                _damagedSlider.value -= Time.deltaTime * _speed;
                if (_damagedSlider.value < _healthSlider.value) _damagedSlider.value = _healthSlider.value;
            }
        }

        void OnDisable()
        {
            _healthEntity.OnHealthChanged -= HandleHealthChanged;
            _healthEntity.MaxHealth.OnValueChanged -= HandleHealthChanged;
        }

        void HandleHealthChanged()
        {
            if (_damagedSlider != null && _damagedSlider.value < _healthSlider.value)
            {
                _damagedSlider.value = _healthSlider.value;
            }
            if (_animator != null && _healthSlider.value < _healthEntity.Health / _healthEntity.MaxHealth.Value)
            {
                _animator.Play("Heal");
            }
            _healthSlider.value = _healthEntity.Health / _healthEntity.MaxHealth.Value;
        }
    }
}
