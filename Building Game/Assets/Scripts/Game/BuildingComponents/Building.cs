using UnityEngine;

namespace Game.BuildingComponents
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private BuildingMovementVisualization _visualization;
        [SerializeField] private Vector2Int _size;
        [SerializeField] private int _index;

        private Vector2 _position;
        private Vector2Int _gridPosition;

        public int Index => _index;
        public Vector2 Position => _position;
        public Vector2Int GridPosition => _gridPosition;
        public Vector2Int Size => _size;

        public void InitializePrefab(int index)
        {
            _index = index;
        }

        public void Initialize(Vector2 position, Vector2Int gridPosition)
        {
            _transform.position = position;
            _position = position;
            _gridPosition = gridPosition;
            _visualization.SetBuildingFinished();
            enabled = true;
        }

        public BuildingInfo GetInfo()
        {
            return new BuildingInfo(Index, Position, GridPosition);
        }

        public void UpdateVisualization(Vector2 position, Vector2 alignedPosition, bool withinSpace, bool available)
        {
            _visualization.UpdateState(position, alignedPosition, withinSpace, available);
        }
    }
}