using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game
{
    public class SoundtrackPlayer : MonoBehaviour
    {
        [SerializeField] Soundtrack _soundtrack;
        [Tooltip("Transition time in seconds")]
        [SerializeField] float _transitionTime;
        [SerializeField] bool _loopSoundtrack = true;

        AudioSource _audioSource;
        AsyncOperationHandle<IList<AudioClip>> _audioClipHandle;

        int _currentTrackIndex = -1;
        float _clipTimeLeft = 0f;
        Coroutine _transitionCoroutine;
        int _lastRandomIndex = -1;
        float _maxVolume;

        void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        void Start()
        {
            _maxVolume = _audioSource.volume;
            _audioClipHandle = _soundtrack.LoadTrackAsync();
        }

        void Update()
        {
            if (!_loopSoundtrack) return;
            _clipTimeLeft -= Time.deltaTime;
            if (_clipTimeLeft < _transitionTime && _transitionCoroutine == null && _audioClipHandle.IsDone)
            {
                // Play next track
                _currentTrackIndex = _currentTrackIndex + 1 >= _soundtrack.Count ? 0 : _currentTrackIndex + 1;
                _transitionCoroutine = StartCoroutine(TransitionToClipCoroutine());
            }
        }

        void OnDestroy()
        {
            if (_audioClipHandle.IsValid())
            {
                Addressables.Release(_audioClipHandle);
            }
        }

        public void PlaySoundtrack(Soundtrack newSoundtrack, bool transition = true)
        {
            if (_soundtrack == newSoundtrack) return;
            if (_audioClipHandle.IsValid())
            {
                Addressables.Release(_audioClipHandle);
            }
            _soundtrack = newSoundtrack;
            _audioClipHandle = _soundtrack.LoadTrackAsync();
            _currentTrackIndex = 0;

            if (_transitionCoroutine != null)
            {
                StopCoroutine(_transitionCoroutine);
                _transitionCoroutine = null;
                _audioSource.volume = _maxVolume;
            }
            _transitionCoroutine = StartCoroutine(TransitionToClipCoroutine(transition));
        }

        public void PlayRandom()
        {
            _currentTrackIndex = Random.Range(0, _soundtrack.Count);
            if (_currentTrackIndex == _lastRandomIndex)
            {
                _currentTrackIndex = _currentTrackIndex + 1 < _soundtrack.Count
                    ? _currentTrackIndex + 1
                    : 0;
            }
            _lastRandomIndex = _currentTrackIndex;

            _transitionCoroutine = StartCoroutine(TransitionToClipCoroutine(false));
        }

        IEnumerator TransitionToClipCoroutine(bool fadeOut = true)
        {
            // Fade Out
            _maxVolume = Mathf.Max(_maxVolume, _audioSource.volume);
            float timeLeft = _transitionTime;
            if (fadeOut)
            {
                while (timeLeft > 0f && _audioSource.isPlaying)
                {
                    timeLeft -= Time.deltaTime;
                    _audioSource.volume = Mathf.InverseLerp(0f, _transitionTime, timeLeft) * _maxVolume;
                    yield return null;
                }
            }

            // Play new clip
            if (!_audioClipHandle.IsDone) yield return _audioClipHandle;
            _audioSource.volume = _maxVolume;
            _audioSource.clip = _audioClipHandle.Result[_currentTrackIndex];
            _audioSource.Play();

            _clipTimeLeft = _audioSource.clip.length;

            // Fade In
            timeLeft = _transitionTime;
            while (timeLeft > 0f && _audioSource.isPlaying)
            {
                timeLeft -= Time.deltaTime;
                _audioSource.volume = _maxVolume - Mathf.InverseLerp(0f, _transitionTime, timeLeft) * _maxVolume;
                yield return null;
            }
            _transitionCoroutine = null;
        }
    }
}
