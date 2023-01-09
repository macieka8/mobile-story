using UnityEngine;
using System;

namespace Game
{
    public class PersistantPosition : MonoBehaviour, IPersistant
    {
        [Serializable]
        public struct SerializablePosition
        {
            public float x, y;
        }

        public void Load(object data, IGameDataHandler dataHandler)
        {
            var position = dataHandler.ToObject<SerializablePosition>(data);
            transform.position = new Vector3(position.x, position.y, transform.position.z);
        }

        public object Save()
        {
            return new SerializablePosition()
            {
                x = transform.position.x,
                y = transform.position.y
            };
        }
    }
}
