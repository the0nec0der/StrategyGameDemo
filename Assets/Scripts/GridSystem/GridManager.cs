using System.Collections.Generic;
using System.Linq;

using Core.InstanceSystem;

using Enums;

using UnityEngine;

namespace GridSystem
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance => Instanced<GridManager>.Instance;

        [Header("Grid Settings")]
        [SerializeField] private GridLayoutType layoutType;
        [SerializeField] private GridOrientation orientation = GridOrientation.Horizontal;
        [SerializeField, Range(1, 10)] private int tileSize = 6;
        [SerializeField, Range(3, 50)] private int gridWidth = 16;
        [SerializeField, Range(3, 50)] private int gridHeight = 9;
        [SerializeField, Range(0, 6)] private int obstacleWeight = 3;

        [Header("Prefabs")]
        [SerializeField] private NodeBase squarePrefab;
        [SerializeField] private NodeBase hexFlatPrefab;
        [SerializeField] private NodeBase hexPointyPrefab;

        public Dictionary<Vector2, NodeBase> Tiles { get; private set; }

        private NodeBase originNode;
        private NodeBase destinationNode;

        private void Awake()
        {
            if (Instance != this)
                return;
        }

        private void Start()
        {
            Tiles = GenerateGrid(layoutType, tileSize, orientation);

            foreach (var tile in Tiles.Values)
                tile.CacheNeighbors();

            originNode = Tiles
                .Where(t => t.Value.Walkable)
                .OrderBy(_ => Random.value)
                .First().Value;

            NodeBase.OnHoverTile += OnTileHover;
        }

        private void OnDestroy()
        {
            NodeBase.OnHoverTile -= OnTileHover;
        }

        private void OnTileHover(NodeBase nodeBase)
        {
            destinationNode = nodeBase;

            foreach (var tile in Tiles.Values)
                tile.RevertTile();

            var path = Pathfinding.FindPath(originNode, destinationNode);
        }

        public NodeBase GetTileAtPosition(Vector2 pos)
        {
            return Tiles.TryGetValue(pos, out var tile) ? tile : null;
        }

        private Dictionary<Vector2, NodeBase> GenerateGrid(GridLayoutType layout, int tileSize, GridOrientation orientation)
        {
            var tiles = new Dictionary<Vector2, NodeBase>();
            var gridParent = new GameObject($"{layout} Grid");

            switch (layout)
            {
                case GridLayoutType.Square:
                    GenerateSquareGrid(tiles, gridParent.transform);
                    break;

                case GridLayoutType.HexFlatTop:
                    GenerateHexFlatTopGrid(tiles, gridParent.transform);
                    break;

                case GridLayoutType.HexPointyTop:
                    GenerateHexPointyTopGrid(tiles, gridParent.transform);
                    break;
            }

            gridParent.transform.localScale = Vector3.one * tileSize;
            gridParent.transform.rotation = orientation == GridOrientation.Horizontal
                ? Quaternion.Euler(90f, 0f, 0f)
                : Quaternion.identity;

            return tiles;
        }

        private void GenerateSquareGrid(Dictionary<Vector2, NodeBase> tiles, Transform parent)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    var tile = Instantiate(squarePrefab, parent);
                    tile.Init(DecideIfObstacle(), new SquareCoords { Pos = new Vector2(x, y) });
                    tiles[new Vector2(x, y)] = tile;

                    tile.name = $"{squarePrefab.name} {x:00} - {y:00}";
                }
            }
        }

        private void GenerateHexFlatTopGrid(Dictionary<Vector2, NodeBase> tiles, Transform parent)
        {
            for (int r = 0; r < gridHeight; r++)
            {
                for (int q = 0; q < gridWidth; q++)
                {
                    var tile = Instantiate(hexFlatPrefab, parent);
                    tile.Init(DecideIfObstacle(), new HexFlatCoords(q, r));
                    tiles[tile.Coords.Pos] = tile;

                    tile.name = $"{hexFlatPrefab.name} {r:00} - {q:00}";
                }
            }
        }

        private void GenerateHexPointyTopGrid(Dictionary<Vector2, NodeBase> tiles, Transform parent)
        {
            for (int r = 0; r < gridHeight; r++)
            {
                int rOffset = r >> 1;
                for (int q = -rOffset; q < gridWidth - rOffset; q++)
                {
                    var tile = Instantiate(hexPointyPrefab, parent);
                    tile.Init(DecideIfObstacle(), new HexPointyCoords(q, r));
                    tiles[tile.Coords.Pos] = tile;

                    tile.name = $"{hexPointyPrefab.name} {r:00} - {q:00}";
                }
            }
        }

        private bool DecideIfObstacle()
        {
            return Random.Range(1, 20) > obstacleWeight;
        }
    }
}
