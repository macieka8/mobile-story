using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;

namespace Game
{
    public class ActivablesBinder : MonoBehaviour, IPersistant
    {
        struct PersistantData
        {
            public List<string> ActivableNames;
        }

        [SerializeField] List<InputActionReference> _inputActions;
        [SerializeReference] List<ActivableReference> _initalActivables;

        List<IActivable> _activables = new List<IActivable>();
        List<Action<InputAction.CallbackContext>> _subscribedDelegates = new List<Action<InputAction.CallbackContext>>();

        public event Action<int> OnActivableChanged;

        public List<IActivable> Activables => _activables;

        void Awake()
        {
            _activables = new List<IActivable>(4);
            for (int i = 0; i < 4; i++)
            {
                _activables.Add(null);
                if (i < _initalActivables.Count)
                    SetActivableSlot(i, _initalActivables[i].GetActivable());
                else
                    SetActivableSlot(i, null);
            }
        }

        void OnEnable()
        {
            _subscribedDelegates = new List<Action<InputAction.CallbackContext>>();
            for (int i = 0; i < _activables.Count; i++)
            {
                int index = i;
                Action<InputAction.CallbackContext> bindedDelegate =
                    delegate (InputAction.CallbackContext ctx)
                    {
                        _activables[index]?.Activate(gameObject);
                    };
                _inputActions[i].action.performed += bindedDelegate;
                _subscribedDelegates.Add(bindedDelegate);
            }
        }

        void OnDisable()
        {
            for (int i = 0; i < _subscribedDelegates.Count; i++)
            {
                _inputActions[i].action.performed -= _subscribedDelegates[i];
            }
        }

        void HandleActivableRemoved(IActivable activable)
        {
            var activableIndex = _activables.IndexOf(activable);
            SetActivableSlot(activableIndex, null);
        }

        public void AddInitalActivable(ActivableReference activableReference)
        {
            _initalActivables.Add(activableReference);
        }

        public void SetActivableSlot(int slotIndex, IActivable activable)
        {
            if (_activables[slotIndex] != null)
                _activables[slotIndex].OnActivableRemove -= HandleActivableRemoved;

            _activables[slotIndex] = activable;
            if (activable != null)
                activable.OnActivableRemove += HandleActivableRemoved;

            OnActivableChanged?.Invoke(slotIndex);
        }

        public void RemoveActivable(int slotIndex)
        {
            SetActivableSlot(slotIndex, null);
        }

        public object Save()
        {
            var names = new List<string>(_activables.Count);
            foreach (var activable in _activables)
            {
                if (activable == null)
                    names.Add("");
                else
                    names.Add(activable.name);
            }

            return new PersistantData
            {
                ActivableNames = names
            };
        }

        public void Load(object data, IGameDataHandler dataHandler)
        {
            var decodedData = dataHandler.ToObject<PersistantData>(data);

            _activables = new List<IActivable>(4);
            while (_activables.Count < 4)
                _activables.Add(null);

            for (int i = 0; i < decodedData.ActivableNames.Count; i++)
            {
                var name = decodedData.ActivableNames[i];
                var index = i;

                if (string.IsNullOrEmpty(name)) continue;
                
                Addressables.LoadAssetAsync<IActivable>(name)
                    .Completed += (AsyncOperationHandle<IActivable> handle) =>
                {
                    SetActivableSlot(index, handle.Result);
                };
            }
        }
    }
}
