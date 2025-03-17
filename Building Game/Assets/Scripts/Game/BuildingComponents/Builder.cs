using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.BuildingComponents
{
    public class Builder : MonoBehaviour
    {
        [SerializeField] private BuildingSpace _buildingSpace;

        private Dictionary<int, Building> _buildingAssociations;
        private Building _chosenPrefab;
        private Building _building;
        private Vector2 _position;

        private bool _isBuilding;

        private bool InitializedPrefab => _chosenPrefab != null;
        private bool InBuildingProcess => _isBuilding && _building != null;

        public void Initialize(Building[] buildings, BuildingInfo[] buildingsInfo)
        {
            _buildingAssociations = new Dictionary<int, Building>();
            foreach (var building in buildings)
            {
                _buildingAssociations.Add(building.Index, building);
            }
            foreach (var building in buildingsInfo)
            {
                Build(building);
            }
        }

        public void ChooseBuilding(Building building)
        {
            if (_buildingAssociations.ContainsKey(building.Index) == false) throw new InvalidOperationException("Trying to choose a building that is not in the internal list");

            StopBuilding();
            _chosenPrefab = building;
            if (_isBuilding && _building == null)
            {
                _building = CreateBuilding(_chosenPrefab, _position);
            }
        }

        public void SetBuilding(bool isBuilding)
        {
            if (_isBuilding == isBuilding) return;

            _isBuilding = isBuilding;
            if (_isBuilding == false)
            {
                StopBuilding();
            }
            else if (InitializedPrefab && _building == null)
            {
                _building = CreateBuilding(_chosenPrefab, _position);
            }
        }

        public void StopBuilding()
        {
            if (_building != null) Destroy(_building.gameObject);
            _building = null;
        }

        public void UpdatePosition(Vector2 position)
        {
            _position = position;
            if (InBuildingProcess == false) return;

            var withinSpace = _buildingSpace.CheckWithinSpace(position);
            var available = _buildingSpace.CheckAvailability(position, _chosenPrefab.Size);
            var alignedPosition = _buildingSpace.GetAlignedPosition(position, _chosenPrefab.Size);
            _building.UpdateVisualization(position, alignedPosition, withinSpace, available);
        }

        public void TryInteract(Vector2 position)
        {
            if (_isBuilding)
            {
                TryFinishBuilding();
            }
            else
            {
                TryDestroy(position);
            }
        }

        private void TryDestroy(Vector2 position)
        {
            _buildingSpace.TryDestroyBuilding(position);
        }

        private void TryFinishBuilding()
        {
            if (InBuildingProcess == false) return;
            if (_buildingSpace.CheckWithinSpace(_position) == false) return;
            if (_buildingSpace.CheckAvailability(_position, _chosenPrefab.Size) == false) return;
            
            _buildingSpace.InitializeBuilding(_building, _position);
            _building = CreateBuilding(_chosenPrefab, _position);
        }

        private void Build(BuildingInfo buildingInfo)
        {
            var building = CreateBuilding(_buildingAssociations[buildingInfo.Index]);
            var position = new Vector2(buildingInfo.PositionX, building.Position.y);
            _buildingSpace.InitializeBuilding(building, position, buildingInfo.GridPosition);
        }

        private Building CreateBuilding(Building prefab)
        {
            return Instantiate(prefab, Vector2.zero, Quaternion.identity);
        }

        private Building CreateBuilding(Building prefab, Vector2 position)
        {
            return Instantiate(prefab, position, Quaternion.identity);
        }
    }
}