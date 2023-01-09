using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Unity.Burst;
using System.Collections;

namespace Game
{
    public class Pathfinding : MonoBehaviour
    {
        const int MOVE_DIAGONAL_COST = 14;
        const int MOVE_STRAIGHT_COST = 10;
        const int PATHFINDING_MAX_ITERATION_COUNT = 4;
        NativeArray<Vector2Int> _neighborOffsets;

        PathfindingGrid _grid;

        void Awake()
        {
            _grid = GetComponent<PathfindingGrid>();
            _neighborOffsets = new NativeArray<Vector2Int>(8, Allocator.Persistent);
            _neighborOffsets[0] = new Vector2Int(-1, -1);
            _neighborOffsets[1] = new Vector2Int(0, -1);
            _neighborOffsets[2] = new Vector2Int(1, -1);
            _neighborOffsets[3] = new Vector2Int(-1, 0);
            _neighborOffsets[4] = new Vector2Int(1, 0);
            _neighborOffsets[5] = new Vector2Int(-1, 1);
            _neighborOffsets[6] = new Vector2Int(0, 1);
            _neighborOffsets[7] = new Vector2Int(1, 1);
        }

        void OnDestroy()
        {
            _neighborOffsets.Dispose();
        }

        public void StartFindPath(PathRequest request)
        {
            StartCoroutine(FindPathCoroutine(request));
        }

        IEnumerator FindPathCoroutine(PathRequest request)
        {
            // Check if valid position
            var startCell = _grid.GetCell(request.StartPosition);
            var endCell = _grid.GetCell(request.EndPosition);

            if (startCell == null || endCell == null)
            {
                request.Callback?.Invoke(new PathResponse { Success = false });
                yield break;
            }
            // startCell == endCell
            else if (startCell.Value.Equals(endCell.Value))
            {
                request.Callback?.Invoke(new PathResponse { Success = true });
                yield break;
            }

            // Schedule job
            var findPathJob = new FindPathJob
            {
                cells = _grid.GetCellsArray(),
                gridResolution = _grid.Resolution,
                startCellIndex = startCell.Value.Coords,
                endCellIndex = endCell.Value.Coords,
                neighborOffsets = _neighborOffsets,
                resultWaypoints = new NativeList<Vector2>(Allocator.TempJob)
            };
            var jobHandler = findPathJob.Schedule();

            // Wait for completion
            int currentIteration = 0;
            while (!jobHandler.IsCompleted)
            {
                currentIteration++;
                if (currentIteration > PATHFINDING_MAX_ITERATION_COUNT)
                 break;
                yield return null;
            }
            jobHandler.Complete();

            // Check if path was found
            if (findPathJob.resultWaypoints.Length < 1)
                request.Callback?.Invoke(new PathResponse { Success = false });

            // Return succesfull response
            request.Callback?.Invoke(new PathResponse
            {
                Success = true,
                Waypoints = findPathJob.resultWaypoints.ToArray()
            });

            findPathJob.resultWaypoints.Dispose();
        }

        [BurstCompile]
        struct FindPathJob : IJob
        {
            [ReadOnly] public NativeArray<Cell> cells;
            [ReadOnly] public Vector2Int gridResolution;
            [ReadOnly] public Vector2Int startCellIndex;
            [ReadOnly] public Vector2Int endCellIndex;
            [ReadOnly] public NativeArray<Vector2Int> neighborOffsets;
            public NativeList<Vector2> resultWaypoints;

            public void Execute()
            {
                FindPath();
            }

            bool IsCoordInGridBounds(Vector2Int coords)
            {
                return coords.x >= 0 && coords.x < gridResolution.x && coords.y >= 0 && coords.y < gridResolution.y;
            }

            void FindPath()
            {
                var startCell = cells[startCellIndex.x + (startCellIndex.y * gridResolution.x)];
                var endCell = cells[endCellIndex.x + (endCellIndex.y * gridResolution.x)];
                if (startCell.CellType == CellType.Wall || endCell.CellType == CellType.Wall) return;

                var pathfindingCells = CreatePathfindingGrid();
                var visitedSet = new NativeHashSet<int>(100, Allocator.Temp);
                var openSet = new MinHeap(gridResolution.x * gridResolution.y, pathfindingCells);

                pathfindingCells[startCell.CellIndex] = openSet.Add(pathfindingCells[startCell.CellIndex]);

                while (openSet.Length > 0)
                {
                    // Find cell with minimmun cost
                    var currentCell = openSet.RemoveFirst();
                    var currentCellIndex = currentCell.CellIndex;

                    // Found end node
                    if (currentCellIndex == endCell.CellIndex)
                    {
                        break;
                    }

                    visitedSet.Add(currentCellIndex);

                    // Handle neighboring nodes
                    for (int i = 0; i < neighborOffsets.Length; i++)
                    {
                        //Check if neighbor in bounds
                        Vector2Int neighborCoords = new Vector2Int(
                            cells[currentCell.CellIndex].Coords.x + neighborOffsets[i].x,
                            cells[currentCell.CellIndex].Coords.y + neighborOffsets[i].y);
                        if (!IsCoordInGridBounds(neighborCoords)) continue;

                        var neighborCell = pathfindingCells[neighborCoords.x + (neighborCoords.y * gridResolution.x)];

                        // Skip visited or unwalkable cells
                        if (visitedSet.Contains(neighborCell.CellIndex) || cells[neighborCell.CellIndex].CellType == CellType.Wall) continue;

                        var newCost = currentCell.gCost + Distance(cells[currentCell.CellIndex], cells[neighborCell.CellIndex]);
                        var isNeighborInOpenSet = openSet.Contains(neighborCell);

                        // Update neighbor cost and parent
                        if (newCost < neighborCell.gCost || !isNeighborInOpenSet)
                        {
                            neighborCell.gCost = newCost;
                            neighborCell.hCost = Distance(cells[neighborCell.CellIndex], endCell) + cells[neighborCell.CellIndex].MovementPenalty;
                            neighborCell.PreviousCellIndex = currentCellIndex;
                            pathfindingCells[neighborCell.CellIndex] = neighborCell;

                            // Add neighbor to openSet
                            if (!isNeighborInOpenSet)
                            {
                                openSet.Add(neighborCell);
                            }
                            else
                            {
                                openSet.UpdateItem(neighborCell);
                            }
                        }
                    }
                }

                // If path was found
                var pathfindingEndCell = pathfindingCells[endCell.CellIndex];
                if (pathfindingEndCell.PreviousCellIndex != - 1)
                {
                    var waypoints = TracebackPath(pathfindingCells[endCell.CellIndex], pathfindingCells);
                    resultWaypoints.AddRange(waypoints);
                }
            }

            NativeArray<PathfindingCell> CreatePathfindingGrid()
            {
                var grid = new NativeArray<PathfindingCell>(cells.Length, Allocator.Temp);
                for (int i = 0; i < cells.Length; i++)
                {
                    grid[i] = new PathfindingCell()
                    {
                        CellIndex = cells[i].CellIndex,
                        PreviousCellIndex = -1,
                        HeapIndex = -1
                    };
                }
                return grid;
            }

            NativeArray<Vector2> TracebackPath(PathfindingCell endCell, NativeArray<PathfindingCell> pathfindingCells)
            {
                if (endCell.PreviousCellIndex == -1)
                    return new NativeArray<Vector2>();

                var path = new NativeList<Vector2Int>(Allocator.Temp);

                var currentCell = endCell;
                while (currentCell.PreviousCellIndex != -1)
                {
                    path.Add(cells[currentCell.CellIndex].Coords);
                    currentCell = pathfindingCells[currentCell.PreviousCellIndex];
                }

                var waypoints = SimplifyPath(path);

                ReverseArray(waypoints);
                path.Dispose();
                return waypoints;
            }

            void ReverseArray(NativeArray<Vector2> array)
            {
                for (int leftIndex = 0; leftIndex < array.Length / 2; leftIndex++)
                {
                    var rightIndex = array.Length - 1 - leftIndex;
                    var temp = array[leftIndex];
                    array[leftIndex] = array[rightIndex];
                    array[rightIndex] = temp;
                }
            }

            NativeArray<Vector2> SimplifyPath(NativeList<Vector2Int> path)
            {
                var waypoints = new NativeList<Vector2>(Allocator.Temp);
                var oldDirection = Vector2.zero;

                for (int i = 1; i < path.Length; i++)
                {
                    var newDirection = new Vector2(
                        path[i - 1].x - path[i].x,
                        path[i - 1].y - path[i].y);

                    if (oldDirection != newDirection)
                    {
                        var cell = cells[path[i - 1].x + (path[i - 1].y * gridResolution.x)];
                        waypoints.Add(cell.Position);
                    }

                    oldDirection = newDirection;
                }

                var lastCell = cells[path[path.Length - 1].x + (path[path.Length - 1].y * gridResolution.x)];
                waypoints.Add(lastCell.Position);
                return waypoints.AsArray();
            }

            int Distance(Cell a, Cell b)
            {
                var x = Mathf.Abs(a.Coords.x - b.Coords.x);
                var y = Mathf.Abs(a.Coords.y - b.Coords.y);
                if (x > y)
                    return (MOVE_DIAGONAL_COST * y) + (MOVE_STRAIGHT_COST * (x - y));
                return (MOVE_DIAGONAL_COST * x) + (MOVE_STRAIGHT_COST * (y - x));
            }
        }
    }
}
