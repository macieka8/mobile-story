using System;

namespace Game
{
    [Serializable]
    public struct ItemSlot
    {
        public Item Item;
        public int Count;

        public ItemSlot(Item item, int count)
        {
            Item = item;
            Count = count;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Item.GetHashCode();
        }

        public static bool operator==(ItemSlot a, ItemSlot b) => a.Equals(b);
        public static bool operator!=(ItemSlot a, ItemSlot b) => !a.Equals(b);
    }
}