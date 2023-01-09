using System.Collections.Generic;

namespace Game
{
    [System.Serializable]
    public class PersistantObjectData
    {
        public string guid;
        public List<PersistantComponentData> components = new List<PersistantComponentData>();
    }
}
