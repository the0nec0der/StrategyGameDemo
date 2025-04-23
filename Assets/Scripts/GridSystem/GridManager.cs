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
        [SerializeField] private GridTile squarePrefab;
        [SerializeField] private GridTile hexFlatPrefab;
        [SerializeField] private GridTile hexPointyPrefab;

        public Dictionary<Vector2, GridTile> Tiles { get; private set; }

        private GridTile originNode;
        private GridTile destinationNode;

        private void Awake()
        {
            if (Instance != this)
                return;
        }

        private void Start()
        {
            Tiles = GenerateGrid(layoutType, tileSize, orientation);

            CacheAllNeighbors();

            originNode = Tiles
                .Where(t => t.Value.Walkable)
                .OrderBy(_ => Random.value)
                .First().Value;

            GridTile.OnHoverTile += OnTileHover;
        }

        private void OnDestroy()
        {
            GridTile.OnHoverTile -= OnTileHover;
        }

        private void OnTileHover(GridTile hoveredTile)
        {
            destinationNode = hoveredTile;

            foreach (var tile in Tiles.Values)
                tile.RevertTile();

            // var path = Pathfinding.FindPath(originNode, destinationNode);
            // GridTileGroupPlacer.Instance?.UpdatePreview(hoveredTile);
            foreach (var neighbor in hoveredTile.Neighbors)
            {
                Debug.Log(neighbor.gameObject.name);
            }
        }

        public GridTile GetTileAtPosition(Vector2 pos)
        {
            return Tiles.TryGetValue(pos, out var tile) ? tile : null;
        }

        private Dictionary<Vector2, GridTile> GenerateGrid(GridLayoutType layout, int tileSize, GridOrientation orientation)
        {
            var tiles = new Dictionary<Vector2, GridTile>();
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

        private void GenerateSquareGrid(Dictionary<Vector2, GridTile> tiles, Transform parent)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    var tile = Instantiate(squarePrefab, parent);
                    tile.Init(DecideIfObstacle(), new SquareCoordinates { Pos = new Vector2(x, y) });
                    tiles[new Vector2(x, y)] = tile;

                    tile.name = $"{squarePrefab.name} {x:00} - {y:00}";
                }
            }
        }

        private void GenerateHexFlatTopGrid(Dictionary<Vector2, GridTile> tiles, Transform parent)
        {
            for (int r = 0; r < gridHeight; r++)
            {
                for (int q = 0; q < gridWidth; q++)
                {
                    var tile = Instantiate(hexFlatPrefab, parent);
                    tile.Init(DecideIfObstacle(), new HexFlatCoordinates(q, r));
                    tiles[tile.Coords.Pos] = tile;

                    tile.name = $"{hexFlatPrefab.name} {q:00} - {r:00}";
                }
            }
        }

        private void GenerateHexPointyTopGrid(Dictionary<Vector2, GridTile> tiles, Transform parent)
        {
            for (int r = 0; r < gridHeight; r++)
            {
                int rOffset = r >> 1;
                for (int q = -rOffset; q < gridWidth - rOffset; q++)
                {
                    var tile = Instantiate(hexPointyPrefab, parent);
                    tile.Init(DecideIfObstacle(), new HexPointyCoordinates(q, r));
                    tiles[tile.Coords.Pos] = tile;

                    tile.name = $"{hexPointyPrefab.name} {q:00} - {r:00}";
                }
            }
        }

        private void CacheAllNeighbors()
        {
            foreach (var tile in Tiles.Values)
            {
                var neighbors = new List<GridTile>();

                if (layoutType == GridLayoutType.Square)
                {
                    List<Vector2> squareDirs = new List<Vector2> {
                    new(0, 1), new(-1, 0), new(0, -1), new(1, 0),
                    new(1, 1), new(1, -1), new(-1, -1), new(-1, 1)
                    };

                    foreach (var dir in squareDirs)
                    {
                        if (Tiles.TryGetValue(tile.Coords.Pos + dir, out var neighbor))
                            neighbors.Add(neighbor);
                    }
                }
                else
                {
                    foreach (var potential in Tiles.Values)
                    {
                        if (tile != potential && tile.Coords.GetDistance(potential.Coords) == 1)
                            neighbors.Add(potential);
                    }
                }

                tile.Neighbors = neighbors;
            }
        }

        private bool DecideIfObstacle()
        {
            return Random.Range(1, 20) > obstacleWeight;
        }
    }
}
