using System.Collections.Generic;

using UnityEngine;

namespace GridSystem
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance;

        [SerializeField] private GridLayoutAsset scriptableGrid;
        [SerializeField] private bool drawConnections;

        public Dictionary<Vector2, NodeBase> Tiles { get; private set; }

        private NodeBase playerNodeBase, goalNodeBase;

        void Awake() => Instance = this;

        private void Start()
        {
            Tiles = scriptableGrid.GenerateGrid();

            foreach (var tile in Tiles.Values) tile.CacheNeighbors();
            NodeBase.OnHoverTile += OnTileHover;
        }

        private void OnDestroy() => NodeBase.OnHoverTile -= OnTileHover;

        private void OnTileHover(NodeBase nodeBase)
        {
            goalNodeBase = nodeBase;

            foreach (var t in Tiles.Values) t.RevertTile();

            var path = Pathfinding.FindPath(playerNodeBase, goalNodeBase);
        }

        public NodeBase GetTileAtPosition(Vector2 pos) => Tiles.TryGetValue(pos, out var tile) ? tile : null;

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying || !drawConnections) return;
            Gizmos.color = Color.red;
            foreach (var tile in Tiles)
            {
                if (tile.Value.Connection == null) continue;
                Gizmos.DrawLine((Vector3)tile.Key + new Vector3(0, 0, -1), (Vector3)tile.Value.Connection.Coords.Pos + new Vector3(0, 0, -1));
            }
        }
    }
}