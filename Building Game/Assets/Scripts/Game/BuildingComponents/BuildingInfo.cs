using System;
using UnityEngine;

namespace Game.BuildingComponents
{
    [Serializable]
    public class BuildingInfo
    {
        public int Index;
        public float PositionX;
        public float PositionY;
        public Vector2Int GridPosition;

        public BuildingInfo(int buildingIndex, Vector2 position, Vector2Int gridPosition)
        {
            Index = buildingIndex;
            PositionX = position.x;
            PositionY = position.y;
            GridPosition = gridPosition;
        }
    }
}
