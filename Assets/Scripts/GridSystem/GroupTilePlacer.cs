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

        private IBuilding building = null;
        private GridTile centerTile = null;
        private List<GridTile> previewTileList = new();
        private List<GridTile> hoveredGridTiles = new();
        private Dictionary<Vector2, GridTile> tiles = new();
        private Transform tilesOffsetTransform;

        public void UpdatePreview(GridTile hoveredTile)
        {
            if (building == null || previewTileList == null || previewTileList.Count == 0) return;

            tilesTransform.position = hoveredTile.transform.position;

            hoveredGridTiles.Clear();
            bool canPlace = true;

            foreach (var previewTile in previewTileList)
            {
                Vector3 previewWorldPos = previewTile.transform.position;
                GridTile targetTile = GridManager.Instance.GetTileAtPosition(previewWorldPos);

                hoveredGridTiles.Add(targetTile);

                if (targetTile == null || !targetTile.Walkable || targetTile.Occupied)
                {
                    canPlace = false;
                }
            }

            for (int i = 0; i < previewTileList.Count; i++)
            {
                if (hoveredGridTiles[i] != null)
                    previewTileList[i].SetColor(canPlace ? Color.green : Color.red);
            }

        }

        public void GenerateGroupTile(IBuilding buildingReference)
        {
            ClearGroupTiles();

            building = buildingReference;
            tiles = GridManager.Instance.GenerateGrid(tilesOffsetTransform, building.Size.x, building.Size.y, isPreview: true);
            previewTileList = new List<GridTile>(tiles.Values);

            if (previewTileList.Count == 0) return;

            centerTile = previewTileList[GetCenterTileIndex(building.Size)];
            Vector3 localCenter = tilesTransform.InverseTransformPoint(centerTile.transform.position);
            tilesOffsetTransform.localPosition = -localCenter;

            foreach (var tile in previewTileList)
            {
                tile.SetColor(Color.red);
            }
        }

        private void ClearGroupTiles()
        {
            if (tilesOffsetTransform != null)
                Destroy(tilesOffsetTransform.gameObject);

            tilesOffsetTransform = new GameObject("Offset").transform;
            tilesOffsetTransform.SetParent(tilesTransform, false);

            building = null;
            centerTile = null;
            previewTileList.Clear();
            tiles.Clear();
        }

        private int GetCenterTileIndex(Vector2Int size)
        {
            int width = size.x;
            int height = size.y;

            return (width, height) switch
            {
                (1, 1) => 0,
                (1, 2) => 0,
                (2, 2) => 0,
                (2, 3) => 2,
                (3, 3) => 4,
                (3, 4) => 4,
                (4, 4) => 5,
                _ => width * height / 2
            };
        }
    }
}
