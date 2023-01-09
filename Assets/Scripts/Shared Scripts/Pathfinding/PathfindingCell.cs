namespace Game
{
    public struct PathfindingCell : IHeapItem<PathfindingCell>
    {
        public int CellIndex { get; set; }

        // Pathfinding fields
        public int gCost { get; set; }
        public int hCost { get; set; }
        public int fCost => gCost + hCost;

        // Traceback fields
        public int PreviousCellIndex { get; set; }

        // Heap field
        public int HeapIndex { get; set; }
        public int HeapParentIndex => (HeapIndex - 1) / 2;
        public int HeapLeftChildIndex => (2 * HeapIndex) + 1;
        public int HeapRightChildIndex => (2 * HeapIndex) + 2;

        public int CompareTo(PathfindingCell other)
        {
            var compare = fCost.CompareTo(other.fCost);
            if (compare == 0) compare = hCost.CompareTo(other.hCost);
            return compare;
        }

        public bool Equals(PathfindingCell other)
        {
            return CellIndex == other.CellIndex;
        }
    }
}
