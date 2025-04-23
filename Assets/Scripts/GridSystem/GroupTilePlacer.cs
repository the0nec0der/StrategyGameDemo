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
        [SerializeField] private Material tilePreviewMaterialGreen;
        [SerializeField] private Material tilePreviewMaterialRed;
        [SerializeField] private Material buildingPreviewMaterial;

        private IBuilding currentBuilding;
        private GridTile centerPreviewTile;
        private List<GridTile> previewTiles = new();
        private List<GridTile> hoveredTiles = new();
        private Dictionary<Vector2, GridTile> previewTileMap = new();
        private Transform previewOffsetRoot;
        private GameObject buildingPreviewInstance;

        private void Update()
        {
            if (currentBuilding != null && Input.GetMouseButtonDown(0))
                TryPlaceBuilding();
        }

        public void UpdatePreview(GridTile hoveredTile)
        {
            if (currentBuilding == null || previewTiles.Count == 0) return;

            tilesTransform.gameObject.SetActive(true);

            tilesTransform.position = new Vector3(hoveredTile.transform.position.x, tilesTransform.position.y, hoveredTile.transform.position.z);

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
                if (hoveredTiles[i] == null) continue;

                var tile = previewTiles[i];
                var renderer = tile.GetComponentInChildren<Renderer>();
                if (renderer != null)
                    renderer.material = canPlace ? tilePreviewMaterialGreen : tilePreviewMaterialRed;
            }

            UpdateBuildingPreview();
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

        private void UpdateBuildingPreview()
        {
            if (hoveredTiles.Count == 0) return;

            Vector3 center = Vector3.zero;
            int count = 0;

            foreach (var tile in hoveredTiles)
            {
                if (tile == null) continue;
                center += tile.transform.position;
                count++;
            }

            if (count == 0) return;
            center /= count;

            if (buildingPreviewInstance == null)
            {
                buildingPreviewInstance = Instantiate(currentBuilding.Prefab);
                DisablePreviewBehavior(buildingPreviewInstance);
            }

            buildingPreviewInstance.transform.position = center;

            foreach (var renderer in buildingPreviewInstance.GetComponentsInChildren<Renderer>())
            {
                renderer.material = buildingPreviewMaterial;
            }
        }

        private void DisablePreviewBehavior(GameObject instance)
        {
            foreach (var col in instance.GetComponentsInChildren<Collider>())
                col.enabled = false;

            foreach (var mono in instance.GetComponentsInChildren<MonoBehaviour>())
                mono.enabled = false;
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

            Vector3 center = Vector3.zero;
            int count = 0;
            foreach (var tile in hoveredTiles)
            {
                if (tile == null) continue;
                center += tile.transform.position;
                count++;
            }

            center /= count;

            InstantiateBuilding(center);
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

        private void InstantiateBuilding(Vector3 position)
        {
            if (currentBuilding != null)
                BuildingFactory.Instance.CreateBuilding(currentBuilding, position);
        }

        private void ClearPreview()
        {
            if (previewOffsetRoot != null)
                Destroy(previewOffsetRoot.gameObject);

            previewOffsetRoot = new GameObject("Offset").transform;
            previewOffsetRoot.SetParent(tilesTransform, false);

            if (buildingPreviewInstance != null)
            {
                Destroy(buildingPreviewInstance);
                buildingPreviewInstance = null;
            }

            currentBuilding = null;
            centerPreviewTile = null;
            previewTiles.Clear();
            hoveredTiles.Clear();
            previewTileMap.Clear();
            tilesTransform.gameObject.SetActive(false);
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
