using UnityEngine;
using System;
using System.Linq;

namespace Game
{
    public class PersistantObject : MonoBehaviour
    {
        [SerializeField] string _id;
        public string Id => _id;

        [ContextMenu("Generate GUID")]
        void GenerateId() => _id = Guid.NewGuid().ToString();

        public PersistantObjectData Save()
        {
            var state = new PersistantObjectData();

            state.guid = Id;
            foreach (var persistant in GetComponents<IPersistant>())
            {
                state.components.Add(new PersistantComponentData
                {
                    name = persistant.GetType().ToString(),
                    data = persistant.Save()
                });
            }

            return state;
        }

        public void Load(PersistantObjectData objectData, IGameDataHandler dataHandler)
        {
            foreach (var component in GetComponents<IPersistant>())
            {
                var componentName = component.GetType().ToString();
                var loadedComponent = objectData.components.FirstOrDefault(item => item.name == componentName);

                if (loadedComponent != null) component.Load(loadedComponent.data, dataHandler);
            }
        }
    }
}
