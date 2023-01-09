using UnityEngine;
using System;

namespace Game
{
    public class PersistantActivity : MonoBehaviour, IPersistant
    {
        [Serializable]
        public struct SerializableActivity
        {
            public bool Active;
        }

        public void Load(object data, IGameDataHandler dataHandler)
        {
            var loadedData = dataHandler.ToObject<SerializableActivity>(data);
            gameObject.SetActive(loadedData.Active);
        }

        public object Save()
        {
            return new SerializableActivity()
            {
                Active = gameObject.activeInHierarchy
            };
        }
    }
}
