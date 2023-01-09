using UnityEngine;

namespace Game
{
    public enum CellType
    {
        Walkable,
        Destroyable,
        Wall
    }

    public struct Cell
    {
        public int CellIndex { get; set; }
        public CellType CellType { get; set; }
        public Vector2 Position { get; set; }
        public Vector2Int Coords { get; set; }
        public int MovementPenalty { get; set; }
    }
}
