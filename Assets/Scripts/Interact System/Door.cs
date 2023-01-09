using System;
using UnityEngine;

namespace Game
{
    public class Door : MonoBehaviour, IInteractable, IPersistant
    {
        struct PersistantData
        {
            public bool IsOpen;
        }

        [SerializeField] GameObject _doorObject;
        [SerializeField] bool _isOpen;
        
        [SerializeField] AudioClip _openAudioClip;
        [SerializeField] AudioClip _closeAudioClip;
        
        AudioSource _audioSource;

        public bool IsOpen => _isOpen;

        void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        void OnValidate()
        {
            if (_isOpen) OpenDoor();
            else CloseDoor();
        }

        public void UpdateDoorState()
        {
            if (_isOpen) OpenDoor();
            else CloseDoor();
        }

        void OpenDoor()
        {
            _doorObject.SetActive(false);
            if (_audioSource != null)
            {
                _audioSource.clip = _openAudioClip;
                _audioSource.Play();
            }
        }

        void CloseDoor()
        {
            _doorObject.SetActive(true);
            if (_audioSource != null)
            {
                _audioSource.clip = _closeAudioClip;
                _audioSource.Play();
            }
        }

        public void Interact()
        {
            _isOpen = !_isOpen;
            UpdateDoorState();
        }

        public object Save()
        {
            return new PersistantData { IsOpen = _isOpen };
        }

        public void Load(object data, IGameDataHandler dataHandler)
        {
            var isOpen = dataHandler.ToObject<PersistantData>(data);
            _isOpen = isOpen.IsOpen;
            UpdateDoorState();
        }
    }
}
