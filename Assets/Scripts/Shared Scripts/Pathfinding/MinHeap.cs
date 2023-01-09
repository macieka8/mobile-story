using System;
using Unity.Collections;

namespace Game
{
    public struct MinHeap
    {
        NativeArray<PathfindingCell> _pathfindingCells;
        NativeArray<PathfindingCell> _items;
        int _currentItemCount;

        public int Length => _currentItemCount;

        public MinHeap(int maxHeapSize, NativeArray<PathfindingCell> pathfindingCells)
        {
            _currentItemCount = 0;
            _items = new NativeArray<PathfindingCell>(maxHeapSize, Allocator.Temp);
            _pathfindingCells = pathfindingCells;
        }

        public PathfindingCell Add(PathfindingCell item)
        {
            item.HeapIndex = _currentItemCount;
            _items[_currentItemCount++] = item;

            _pathfindingCells[item.CellIndex] = item;

            SortUp(item);
            return item;
        }

        public PathfindingCell RemoveFirst()
        {
            var firstItem = _items[0];

            firstItem.HeapIndex = -1;
            _pathfindingCells[firstItem.CellIndex] = firstItem;

            _currentItemCount--;

            var lastItem = _items[_currentItemCount];
            lastItem.HeapIndex = 0;
            _items[0] = lastItem;

            _pathfindingCells[_items[0].CellIndex] = _items[0];

            SortDown(_items[0]);

            return firstItem;
        }

        public PathfindingCell UpdateItem(PathfindingCell item)
        {
            _items[item.HeapIndex] = item;
            _pathfindingCells[item.CellIndex] = item;
            SortUp(item);
            return item;
        }

        public bool Contains(PathfindingCell item)
        {
            if (item.HeapIndex == -1) return false;
            return _items[item.HeapIndex].Equals(item);
        }

        public void Dispose()
        {
            _items.Dispose();
        }

        void SortUp(PathfindingCell item)
        {
            while (true)
            {
                var parent = _items[item.HeapParentIndex];
                if (parent.CompareTo(item) > 0)
                {
                    Swap(ref item, ref parent);
                }
                else
                {
                    break;
                }
            }
        }

        void SortDown(PathfindingCell item)
        {
            while (true)
            {
                if (item.HeapLeftChildIndex < _currentItemCount)
                {
                    var smallerChildIndex = item.HeapLeftChildIndex;
                    if (item.HeapRightChildIndex < _currentItemCount)
                    {
                        if (_items[item.HeapRightChildIndex].CompareTo(_items[item.HeapLeftChildIndex]) < 0)
                        {
                            smallerChildIndex = item.HeapRightChildIndex;
                        }
                    }

                    if (item.CompareTo(_items[smallerChildIndex]) > 0)
                    {
                        var x = _items[smallerChildIndex];
                        Swap(ref item, ref x);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        void Swap(ref PathfindingCell a, ref PathfindingCell b)
        {
            var temp = a;
            a.HeapIndex = b.HeapIndex;
            b.HeapIndex = temp.HeapIndex;

            _items[a.HeapIndex] = a;
            _items[b.HeapIndex] = b;

            _pathfindingCells[a.CellIndex] = a;
            _pathfindingCells[b.CellIndex] = b;
        }
    }

    public interface IHeapItem<T> : IComparable<T>, IEquatable<T> where T : struct
    {
        public int HeapIndex { get; set; }

        public int HeapParentIndex { get; }
        public int HeapLeftChildIndex { get; }
        public int HeapRightChildIndex { get; }
    }
}
