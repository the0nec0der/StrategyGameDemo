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
            Tiles = GenerateGrid(gridWidth: gridWidth, gridHeight: gridHeight);

            originNode = Tiles
                .Where(t => t.Value.Walkable)
                .OrderBy(_ => Random.value)
                .First().Value;

            GridTile.OnTileHovered += OnTileHovered;
            GridTile.OnTileSelected += OnTileSelected;
        }

        private void OnDestroy()
        {
            GridTile.OnTileHovered -= OnTileHovered;
        }

        private void OnTileSelected(GridTile selectedTile)
        {
            Debug.Log($"selected tile name: {selectedTile.transform.name} \n pos: {selectedTile.transform.position} --- {GetTileAtPosition(selectedTile.transform.position)?.gameObject.name}");
        }

        private void OnTileHovered(GridTile hoveredTile)
        {
            destinationNode = hoveredTile;

            foreach (var tile in Tiles.Values)
                tile.RevertTile();

            // var path = Pathfinding.FindPath(originNode, destinationNode);
            GroupTilePlacer.Instance.UpdatePreview(hoveredTile);
            // foreach (var neighbor in hoveredTile.Neighbors)
            // {
            //     Debug.Log(neighbor.gameObject.name);
            // }
        }

        public GridTile GetTileAtPosition(Vector3 worldPos)
        {
            foreach (var kvp in Tiles)
            {
                Vector2 tilePos = kvp.Key;

                Vector2 projectedWorldPos = orientation == GridOrientation.Horizontal
                    ? new Vector2(worldPos.x, worldPos.z)
                    : new Vector2(worldPos.x, worldPos.y);

                if (Vector2.Distance(tilePos, projectedWorldPos) < 0.1f)
                    return kvp.Value;
            }

            return null;
        }

        public Dictionary<Vector2, GridTile> GenerateGrid(Transform parent = null, int gridWidth = 0, int gridHeight = 0, bool isPreview = false)
        {
            var tiles = new Dictionary<Vector2, GridTile>();
            var gridParent = parent == null ? new GameObject($"{layoutType} Grid").transform : parent;

            switch (layoutType)
            {
                case GridLayoutType.Square:
                    GenerateSquareGrid(tiles, gridParent.transform, gridWidth, gridHeight, isPreview);
                    break;

                case GridLayoutType.HexFlatTop:
                    GenerateHexFlatTopGrid(tiles, gridParent.transform, gridWidth, gridHeight, isPreview);
                    break;

                case GridLayoutType.HexPointyTop:
                    GenerateHexPointyTopGrid(tiles, gridParent.transform, gridWidth, gridHeight, isPreview);
                    break;
            }

            gridParent.transform.rotation = orientation == GridOrientation.Horizontal
                ? Quaternion.Euler(90f, 0f, 0f)
                : Quaternion.identity;

            CacheAllNeighbors(tiles);

            return tiles;
        }

        public void GenerateSquareGrid(Dictionary<Vector2, GridTile> tiles, Transform parent, int gridWidth = 0, int gridHeight = 0, bool isPreview = false)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    var tile = Instantiate(squarePrefab, parent);

                    tile.Init(DecideIfObstacle(isPreview), isPreview, new SquareCoordinates { Pos = new Vector2(x, y) });

                    tiles[new Vector2(x, y)] = tile;

                    tile.name = $"{squarePrefab.name} {x:00} - {y:00}";
                }
            }
        }

        public void GenerateHexFlatTopGrid(Dictionary<Vector2, GridTile> tiles, Transform parent, int gridWidth = 0, int gridHeight = 0, bool isPreview = false)
        {
            for (int r = 0; r < gridHeight; r++)
            {
                for (int q = 0; q < gridWidth; q++)
                {
                    var tile = Instantiate(hexFlatPrefab, parent);
                    tile.Init(DecideIfObstacle(isPreview), isPreview, new HexFlatCoordinates(q, r));
                    tiles[tile.Coords.Pos] = tile;

                    tile.name = $"{hexFlatPrefab.name} {q:00} - {r:00}";
                }
            }
        }

        public void GenerateHexPointyTopGrid(Dictionary<Vector2, GridTile> tiles, Transform parent, int gridWidth = 0, int gridHeight = 0, bool isPreview = false)
        {
            for (int r = 0; r < gridHeight; r++)
            {
                int rOffset = r >> 1;
                for (int q = -rOffset; q < gridWidth - rOffset; q++)
                {
                    var tile = Instantiate(hexPointyPrefab, parent);
                    tile.Init(DecideIfObstacle(isPreview), isPreview, new HexPointyCoordinates(q, r));
                    tiles[tile.Coords.Pos] = tile;

                    tile.name = $"{hexPointyPrefab.name} {q:00} - {r:00}";
                }
            }
        }

        public void CacheAllNeighbors(Dictionary<Vector2, GridTile> tiles)
        {
            foreach (var tile in tiles.Values)
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
                        if (tiles.TryGetValue(tile.Coords.Pos + dir, out var neighbor))
                            neighbors.Add(neighbor);
                    }
                }
                else
                {
                    foreach (var potential in tiles.Values)
                    {
                        if (tile != potential && tile.Coords.GetDistance(potential.Coords) == 1)
                            neighbors.Add(potential);
                    }
                }

                tile.Neighbors = neighbors;
            }
        }

        private bool DecideIfObstacle(bool isPreview)
        {
            if (isPreview)
                return true;

            return Random.Range(1, 20) > obstacleWeight;
        }
    }
}
