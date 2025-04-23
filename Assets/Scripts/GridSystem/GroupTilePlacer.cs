using System.Collections.Generic;

using Core.InstanceSystem;

using Gameplay.Buildings;

using UnityEngine;

namespace GridSystem
{
    public class GroupTilePlacer : MonoBehaviour
    {
        public static GroupTilePlacer Instance => Instanced<GroupTilePlacer>.Instance;

        [SerializeField] private Transform tilesTransform;

        private IBuilding currentBuilding;
        private GridTile centerPreviewTile;
        private List<GridTile> previewTiles = new();
        private List<GridTile> hoveredTiles = new();
        private Dictionary<Vector2, GridTile> previewTileMap = new();
        private Transform previewOffsetRoot;

        private void Update()
        {
            if (currentBuilding != null && Input.GetMouseButtonDown(0))
                TryPlaceBuilding();
        }

        public void UpdatePreview(GridTile hoveredTile)
        {
            if (currentBuilding == null || previewTiles.Count == 0) return;

            tilesTransform.position = hoveredTile.transform.position;

            hoveredTiles.Clear();
            bool canPlace = true;

            foreach (var previewTile in previewTiles)
            {
                var worldPos = previewTile.transform.position;
                var gridTile = GridManager.Instance.GetTileAtPosition(worldPos);
                hoveredTiles.Add(gridTile);

                if (gridTile == null || !gridTile.Walkable || gridTile.Occupied)
                    canPlace = false;
            }

            for (int i = 0; i < previewTiles.Count; i++)
            {
                if (hoveredTiles[i] != null)
                    previewTiles[i].SetColor(canPlace ? Color.green : Color.red);
            }
        }

        public void GenerateGroupTile(IBuilding building)
        {
            ClearPreview();

            currentBuilding = building;
            previewTileMap = GridManager.Instance.GenerateGrid(previewOffsetRoot, building.Size.x, building.Size.y, isPreview: true);
            previewTiles = new List<GridTile>(previewTileMap.Values);

            if (previewTiles.Count == 0) return;

            centerPreviewTile = previewTiles[GetCenterTileIndex(building.Size)];
            Vector3 localCenter = tilesTransform.InverseTransformPoint(centerPreviewTile.transform.position);
            previewOffsetRoot.localPosition = -localCenter;

            foreach (var tile in previewTiles)
                tile.SetColor(Color.red);
        }

        public void TryPlaceBuilding()
        {
            if (hoveredTiles.Count == 0 || !IsPlacementValid())
            {
                Debug.Log("Cannot place building here.");
                return;
            }

            foreach (var tile in hoveredTiles)
            {
                tile.Occupied = true;
                tile.RevertTile();
            }

            InstantiateBuilding();
            ClearPreview();
        }

        private bool IsPlacementValid()
        {
            foreach (var tile in hoveredTiles)
            {
                if (tile == null || !tile.Walkable || tile.Occupied)
                    return false;
            }
            return true;
        }

        private void InstantiateBuilding()
        {
            if (currentBuilding != null)
                BuildingFactory.Instance.CreateBuilding(currentBuilding, tilesTransform.position);
        }

        private void ClearPreview()
        {
            if (previewOffsetRoot != null)
                Destroy(previewOffsetRoot.gameObject);

            previewOffsetRoot = new GameObject("Offset").transform;
            previewOffsetRoot.SetParent(tilesTransform, false);

            currentBuilding = null;
            centerPreviewTile = null;
            previewTiles.Clear();
            hoveredTiles.Clear();
            previewTileMap.Clear();
        }

        private int GetCenterTileIndex(Vector2Int size)
        {
            int w = size.x;
            int h = size.y;

            return (w, h) switch
            {
                (1, 1) => 0,
                (1, 2) => 0,
                (2, 2) => 0,
                (2, 3) => 2,
                (3, 3) => 4,
                (3, 4) => 4,
                (4, 4) => 5,
                _ => w * h / 2
            };
        }
    }
}
