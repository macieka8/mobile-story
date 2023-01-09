using UnityEngine;
using System.Collections;

namespace Game
{
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] ParticleSystem _bloodParticles;
        [SerializeField] ParticleSystem _healParticles;
        [SerializeField] AudioClip _TakeDamageAudioClip;

        SpriteRenderer _spriteRenderer;
        Animator _animator;
        IMovementController _movementController;
        AttackingEntity _attackingEntity;
        IHealthEntity _healthEntity;
        AudioSource _audioSource;

        bool _isAttacking;
        Vector2 _movement;
        float _prevHealth;
        bool _isDead = false;

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _movementController = GetComponent<IMovementController>();
            _attackingEntity = GetComponent<AttackingEntity>();
            _animator = GetComponent<Animator>();
            _healthEntity = GetComponent<IHealthEntity>();
            _audioSource = GetComponent<AudioSource>();
        }

        void Start()
        {
            _prevHealth = _healthEntity.Health;
        }

        void Update()
        {
            if (_isDead) return;
            HandleMovementInput(_movementController.MovementDirection);
        }

        void OnEnable()
        {
            _attackingEntity.OnAttackStart += HandleAttackInput;
            _healthEntity.OnHealthChanged += HandleHealthChanged;
            _healthEntity.OnEntityDead += HandleEntityDead;
        }

        void OnDisable()
        {
            _attackingEntity.OnAttackStart -= HandleAttackInput;
            _healthEntity.OnHealthChanged -= HandleHealthChanged;
            _healthEntity.OnEntityDead -= HandleEntityDead;
        }

        void HandleHealthChanged()
        {
            if (_healthEntity.Health > 0) _isDead = false;
            if (_healthEntity.Health < _prevHealth)
            {
                _bloodParticles.Play();
                _audioSource.clip = _TakeDamageAudioClip;
                _audioSource.Play();
            }
            else if (_healthEntity.Health > _prevHealth)
            {
                if (_healParticles != null)
                    _healParticles.Play();
            }

            _prevHealth = _healthEntity.Health;
        }

        void HandleEntityDead()
        {
            _animator.Play("Death");
            _isDead = true;
        }

        void HandleMovementInput(Vector2 movementInput)
        {
            _movement = movementInput;
            if (_isAttacking) return;
            if (_isDead) return;

            if (movementInput.magnitude > 0.1f) _animator.Play("Walking");
            else _animator.Play("Idle");

            
            if (movementInput.x > 0f) _spriteRenderer.flipX = false;
            else if (movementInput.x < 0f) _spriteRenderer.flipX = true;
        }

        void HandleAttackInput(AttackData attackData)
        {
            if (_isDead) return;
            _animator.Play(attackData.AnimationName);
            _isAttacking = true;
            StartCoroutine(WaitForAttackStop(attackData.CastTime));
        }

        IEnumerator WaitForAttackStop(float waitTimeInSeconds)
        {
            yield return new WaitForSeconds(waitTimeInSeconds);
            _isAttacking = false;
            HandleMovementInput(_movement);
        }
    }
}

