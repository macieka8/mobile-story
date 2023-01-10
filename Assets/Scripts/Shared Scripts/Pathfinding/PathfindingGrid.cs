using Unity.Collections;
using UnityEngine;

namespace Game
{
    public class PathfindingGrid : MonoBehaviour
    {
        [SerializeField] LayerMask _structureLayer;
        [SerializeField] LayerMask _interactableLayer;
        [SerializeField] Vector2Int _gridResolution;
        [SerializeField] float _cellDiameter;

        [Tooltip("Movement penalty kernel radius")]
        [Min(0)][SerializeField] int _kernelRadius;

        [SerializeField] int _emptyPenalty;
        [SerializeField] int _obstacleProximityPenalty;

        Vector2 _gridSize;
        NativeArray<Cell> _cells;
        float _cellRadius;

        int _minMovementPenalty = int.MaxValue;
        int _maxMovementPenalty = int.MinValue;

        public Vector2Int Resolution => _gridResolution;

        public int CellCount => _gridResolution.x * _gridResolution.y;

        void Awake()
        {
            _gridSize = new Vector2(_gridResolution.x * _cellDiameter, _gridResolution.y * _cellDiameter);
            _cellRadius = _cellDiameter / 2f;

            _cells = CreateGrid();
            CalculateBlurMovementPenalty();
        }

        void OnDestroy()
        {
            if (_cells != null)
            {
                _cells.Dispose();
            }
        }

        NativeArray<Cell> CreateGrid()
        {
            var grid = new NativeArray<Cell>(_gridResolution.y * _gridResolution.x, Allocator.Persistent);
            Vector2 bottomLeftPos = transform.position - (_gridSize.x / 2 * Vector3.right) - (_gridSize.y / 2 * Vector3.up);

            for (int y = 0; y < _gridResolution.y; y++)
            {
                for (int x = 0; x < _gridResolution.x; x++)
                {
                    var cellPosition = bottomLeftPos
                        + (((x * _cellDiameter) + _cellRadius) * Vector2.right)
                        + (((y * _cellDiameter) + _cellRadius) * Vector2.up);

                    CellType cellType = DetermineCellType(cellPosition);
                    var coords = new Vector2Int(x, y);
                    var weight = DetermineCellWeight(cellType);

                    grid[x + (y * _gridResolution.x)] = new Cell
                    {
                        CellIndex = x + (y * _gridResolution.x),
                        CellType = cellType,
                        Position = cellPosition,
                        Coords = coords,
                        MovementPenalty = weight,
                    };
                }
            }

            return grid;
        }

        void CalculateBlurMovementPenalty()
        {
            int kernelArea = (_kernelRadius * 2) + 1;
            kernelArea *= kernelArea;

            var horizontalMap = new int[_gridResolution.y, _gridResolution.x];
            var verticalMap = new int[_gridResolution.y, _gridResolution.x];

            // Horizontal sum
            for (int y = 0; y < _gridResolution.y; y++)
            {
                // Calculate first cell
                for (int x = -_kernelRadius; x <= _kernelRadius; x++)
                {
                    var xIndex = Mathf.Clamp(x, 0, _gridResolution.x - 1);
                    horizontalMap[y, 0] += _cells[xIndex + (y * _gridResolution.x)].MovementPenalty;
                }

                // Calculate other cells by adding value on the right and substracting value to the left
                for (int x = 1; x < _gridResolution.x; x++)
                {
                    var leftIndex = Mathf.Clamp(x - _kernelRadius - 1, 0, _gridResolution.x - 1);
                    var rightIndex = Mathf.Clamp(x + _kernelRadius, 0, _gridResolution.x - 1);
                    horizontalMap[y, x] = horizontalMap[y, x - 1]
                        - _cells[leftIndex + (y * _gridResolution.x)].MovementPenalty
                        + _cells[rightIndex + (y * _gridResolution.x)].MovementPenalty;
                }
            }

            // Vertical sum
            for (int x = 0; x < _gridResolution.x; x++)
            {
                // Calculate first cell
                for (int y = -_kernelRadius; y <= _kernelRadius; y++)
                {
                    var yIndex = Mathf.Clamp(y, 0, _gridResolution.y - 1);
                    verticalMap[0, x] += horizontalMap[yIndex, x];
                }

                // Get first cell movement penalty
                var tempCell =_cells[x];
                tempCell.MovementPenalty = Mathf.RoundToInt((float)verticalMap[0, x] / kernelArea);
                _cells[x] = tempCell;
                // Cache Max & Min povement penalties
                if (_cells[x].MovementPenalty > _maxMovementPenalty)
                    _maxMovementPenalty = _cells[x].MovementPenalty;
                if (_cells[x].MovementPenalty < _minMovementPenalty)
                    _minMovementPenalty = _cells[x].MovementPenalty;

                // Calculate other cells by adding value on the top and substracting value to the bottom
                for (int y = 1; y < _gridResolution.y; y++)
                {
                    var bottomIndex = Mathf.Clamp(y - _kernelRadius - 1, 0, _gridResolution.y - 1);
                    var topIndex = Mathf.Clamp(y + _kernelRadius, 0, _gridResolution.y - 1);
                    verticalMap[y, x] = verticalMap[y - 1, x]
                        - horizontalMap[bottomIndex, x]
                        + horizontalMap[topIndex, x];

                    // Save cell movement penalty
                    tempCell = _cells[x + (y * _gridResolution.x)];
                    tempCell.MovementPenalty = Mathf.RoundToInt((float)verticalMap[y, x] / kernelArea);
                    _cells[x + (y * _gridResolution.x)] = tempCell;
                    // Cache Max & Min movement penalties
                    if (_cells[x + (y * _gridResolution.x)].MovementPenalty > _maxMovementPenalty)
                        _maxMovementPenalty = _cells[x + (y * _gridResolution.x)].MovementPenalty;
                    if (_cells[x + (y * _gridResolution.x)].MovementPenalty < _minMovementPenalty)
                        _minMovementPenalty = _cells[x + (y * _gridResolution.x)].MovementPenalty;
                }
            }
        }

        CellType DetermineCellType(Vector2 cellPosition)
        {
            CellType cellType;
            var colliderStructure = Physics2D.OverlapCircle(cellPosition, _cellRadius * 0.9f, _structureLayer);

            if (colliderStructure)
            {
                if (colliderStructure.TryGetComponent<IHealthEntity>(out _))
                    cellType = CellType.Destroyable;
                else if (Physics2D.OverlapPoint(colliderStructure.transform.position, _interactableLayer))
                    cellType = CellType.Walkable;
                else
                    cellType = CellType.Wall;
            }
            else
            {
                cellType = CellType.Walkable;
            }

            return cellType;
        }

        int DetermineCellWeight(CellType cellType)
        {
            if (cellType == CellType.Wall) return _obstacleProximityPenalty;
            else return _emptyPenalty;
        }

        /// <summary>
        /// Return cell in a given world position
        /// </summary>
        public Cell? GetCell(Vector2 position)
        {
            var coordX = (int)((position.x + (_gridSize.x / 2f)) / _cellDiameter);
            var coordY = (int)((position.y + (_gridSize.y / 2f)) / _cellDiameter);
            if (coordX < 0 || coordX >= _gridResolution.x ||
                coordY < 0 || coordY >= _gridResolution.y)
            {
                return null;
            }
            return _cells[coordX + (coordY * _gridResolution.x)];
        }

        public NativeArray<Cell> GetCellsArray()
        {
            return _cells;
        }

        public void RecreateGrid()
        {
            // TODO: Optimize / Recreate only changed parts / chunks
            if (_cells != null)
            {
                _cells.Dispose();
            }
            _cells = CreateGrid();
            CalculateBlurMovementPenalty();
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireCube(
                transform.position,
                new Vector2(_gridResolution.x * _cellDiameter, _gridResolution.y * _cellDiameter));

            if (_cells != null)
            {
                foreach (var cell in _cells)
                {
                    if (cell.CellType == CellType.Wall)
                    {
                        Gizmos.color = Color.red;
                    }
                    else
                    {
                        var scale = (float)(cell.MovementPenalty - _minMovementPenalty) / (_maxMovementPenalty - _minMovementPenalty);
                        Gizmos.color = Color.Lerp(Color.white, Color.black, scale);
                    }

                    Gizmos.DrawCube(cell.Position, Vector2.one * (_cellDiameter * 0.9f));
                }
            }
        }
    }
}
