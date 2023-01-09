using System.Collections.Generic;

namespace Game
{
    [System.Serializable]
    public class GameData
    {
        public List<PersistantObjectData> objects = new List<PersistantObjectData>();
        public object story;
    }
}
