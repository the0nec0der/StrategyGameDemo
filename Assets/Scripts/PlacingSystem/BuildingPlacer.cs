using System.Collections.Generic;

using Gameplay.Buildings;

using GridSystem;

using UnityEngine;

namespace PlacingSystem
{
    public class BuildingPlacer : BasePlacer<IBuilding>
    {
        private IBuilding currentBuilding;

        public void StartPlacingBuilding(IBuilding building)
        {
            ClearPreview();

            currentBuilding = building;
            previewTileMap = GridManager.Instance.GenerateGrid(previewOffsetRoot, building.Size.x, building.Size.y, isPreview: true);
            previewTiles = new List<GridTile>(previewTileMap.Values);

            if (previewTiles.Count == 0) return;

            centerPreviewTile = previewTiles[GetCenterTileIndex(building.Size)];
            Vector3 localCenter = tilesTransform.InverseTransformPoint(centerPreviewTile.transform.position);
            previewOffsetRoot.localPosition = -localCenter;
        }

        public override void OnTileHovered(GridTile tile)
        {
            lastHoveredTile = tile;
            tilesTransform.position = new Vector3(tile.transform.position.x, tilesTransform.position.y, tile.transform.position.z);

            hoveredTiles.Clear();
            foreach (var previewTile in previewTiles)
            {
                var gridTile = GridManager.Instance.GetTileAtPosition(previewTile.transform.position);
                hoveredTiles.Add(gridTile);
            }

            HighlightTiles(IsPlacementValid());
            UpdatePreview();
        }

        protected override void UpdatePreview()
        {
            if (hoveredTiles.Count == 0) return;

            tilesTransform.gameObject.SetActive(true);

            Vector3 center = GetCenterOfPreviewTiles();

            if (productPreviewInstance == null)
            {
                productPreviewInstance = Instantiate(currentBuilding.Prefab, center, Quaternion.Euler(0f, rotationStep * RotationIncrement, 0f), tilesTransform);
                DisablePreviewBehavior(productPreviewInstance);
            }

            productPreviewInstance.transform.position = center;
            productPreviewInstance.transform.rotation = Quaternion.Euler(0f, rotationStep * RotationIncrement, 0f);

            foreach (var renderer in productPreviewInstance.GetComponentsInChildren<Renderer>())
                renderer.material = productPreviewMaterialTransparent;
        }

        public override bool OnConfirmPlacement()
        {
            if (!base.OnConfirmPlacement())
                return false;

            SeTilesColor(currentBuilding.OccupiedGradient.Evaluate(Random.Range(0f, 1f)));

            Vector3 center = GetCenterOfPreviewTiles();

            var building = GameLogicMediator.Instance.BuildingFactory.CreateBuilding(currentBuilding, center);
            building.transform.rotation = Quaternion.Euler(0f, rotationStep * RotationIncrement, 0f);
            ClearPreview();

            return true;
        }

        protected override void ClearPreview()
        {
            base.ClearPreview();
            currentBuilding = null;
        }

        protected override void DisablePreviewBehavior(GameObject instance)
        {
            foreach (var col in instance.GetComponentsInChildren<Collider>())
                col.enabled = false;

            foreach (var mono in instance.GetComponentsInChildren<MonoBehaviour>())
                mono.enabled = false;
        }
    }
}
