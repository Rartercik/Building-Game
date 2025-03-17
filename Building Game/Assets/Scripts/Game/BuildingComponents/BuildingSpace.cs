using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.BuildingComponents
{
    public class BuildingSpace : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Vector2Int _size;
        [SerializeField] private float _cellSize = 1f;

        private Building[,] _cellsOccupation;
        private List<Building> _buildings;

        public Bounds Bounds => _renderer.bounds;

        private Vector2 LeftDownPosition => new Vector2(Bounds.min.x, Bounds.min.y);
        private Vector2 RightUpPosition => new Vector2(Bounds.max.x, Bounds.max.y);

        public void Initialize()
        {
            _cellsOccupation = new Building[_size.x, _size.y];
            _buildings = new List<Building>();
        }

        public bool CheckWithinSpace(Vector2 position)
        {
            return Bounds.Contains(position);
        }

        public bool CheckAvailability(Vector2 position, Vector2Int size)
        {
            var gridPosition = ConvertToGridSpace(position, size);
            return CheckCellsAreAvailable(gridPosition, size);
        }

        public void InitializeBuilding(Building building, Vector2 position)
        {
#if UNITY_EDITOR
            if (CheckWithinSpace(position) == false) throw new System.ArgumentOutOfRangeException("Position is not within the building space");
            if (CheckAvailability(position, building.Size) == false) throw new System.InvalidOperationException("Trying to initialize a building on an unavailable area");
#endif

            var gridPosition = ConvertToGridSpace(position, building.Size);
            InitializeBuilding(building, position, gridPosition);
        }

        public void InitializeBuilding(Building building, Vector2 position, Vector2Int gridPosition)
        {
            _buildings.Add(building);
            position = ConvertToWorldSpace(gridPosition, building.Size);
            building.Initialize(position, gridPosition);
            SetOccupation(gridPosition, building.Size, building);
        }

        public void TryDestroyBuilding(Vector2 position)
        {
            if (CheckWithinSpace(position) == false) return;

            var gridPosition = ConvertToGridSpace(position, Vector2Int.one);
            var building = _cellsOccupation[gridPosition.x, gridPosition.y];
            if (building == null) return;

            _buildings.Remove(building);
            SetOccupation(building.GridPosition, building.Size, null);
            Destroy(building.gameObject);
        }

        public Vector2 GetAlignedPosition(Vector2 position, Vector2Int size)
        {
            var gridPosition = ConvertToGridSpace(position, size);
            return ConvertToWorldSpace(gridPosition, size);
        }

        public BuildingInfo[] GetBuildingsInfo()
        {
            return _buildings.Select(building => building.GetInfo()).ToArray();
        }

        private Vector2Int ConvertToGridSpace(Vector2 position, Vector2Int size)
        {
            var vectorToPosition = position - LeftDownPosition;
            var vectorToCorner = RightUpPosition - LeftDownPosition;
            var ratioX = vectorToPosition.x / vectorToCorner.x;
            var ratioY = vectorToPosition.y / vectorToCorner.y;
            var x = Mathf.RoundToInt(ratioX * _cellsOccupation.GetLength(0) - size.x / 2f);
            var y = Mathf.RoundToInt(ratioY * _cellsOccupation.GetLength(1) - size.y / 2f);

            return new Vector2Int(x, y);
        }

        private Vector2 ConvertToWorldSpace(Vector2Int position, Vector2Int size)
        {
            var ratioX = (float)position.x / _cellsOccupation.GetLength(0);
            var ratioY = (float)position.y / _cellsOccupation.GetLength(1);
            var x = LeftDownPosition.x + Bounds.size.x * ratioX + size.x * _cellSize / 2f;
            var y = LeftDownPosition.y + Bounds.size.y * ratioY + size.y * _cellSize / 2f;

            return new Vector2(x, y);
        }

        private bool CheckWithinGrid(Vector2Int position)
        {
            return position.x >= 0
                && position.x < _cellsOccupation.GetLength(0)
                && position.y >= 0
                && position.y < _cellsOccupation.GetLength(1);
        }

        private bool CheckCellsAreAvailable(Vector2Int position, Vector2Int size)
        {
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    var x = position.x + i;
                    var y = position.y + j;

                    if (CheckWithinGrid(new Vector2Int(x, y)) == false || _cellsOccupation[x, y] != null) return false;
                }
            }

            return true;
        }

        private void SetOccupation(Vector2Int position, Vector2Int size, Building building)
        {
            for (int y = position.y; y < position.y + size.y; y++)
            {
                for (int x = position.x; x < position.x + size.x; x++)
                {
                    _cellsOccupation[x, y] = building;
                }    
            }
        }
    }
}