using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Items/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] [TextArea] string _description;
        [SerializeField] Sprite _icon;
        [SerializeField] [Min(1)] int _maxStackCount = 1;

        public string Description => _description;
        public Sprite Icon => _icon;
        public int MaxStackCount => _maxStackCount;
    }
}