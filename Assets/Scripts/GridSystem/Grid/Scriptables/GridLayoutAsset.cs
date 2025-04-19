using System.Collections.Generic;

using UnityEngine;

namespace GridSystem
{
    public abstract class GridLayoutAsset : ScriptableObject
    {
        [SerializeField] protected NodeBase nodeBasePrefab;
        [SerializeField, Range(3, 50)] protected int gridWidth = 16;
        [SerializeField, Range(3, 50)] protected int gridHeight = 9;
        [SerializeField, Range(0, 6)] private int obstacleWeight = 3;
        public abstract Dictionary<Vector2, NodeBase> GenerateGrid();

        protected bool DecideIfObstacle() => Random.Range(1, 20) > obstacleWeight;
    }
}
