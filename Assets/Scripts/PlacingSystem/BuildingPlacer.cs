using System.Collections.Generic;

using Gameplay;
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
            GameStateManager.SetState(Enums.GameStateType.BuildingPlacement);
            ClearPreview();

            currentBuilding = building;
            previewTileMap = GridManager.GenerateGrid(previewOffsetRoot, building.Size.x, building.Size.y, isPreview: true);
            previewTiles = new List<GridTile>(previewTileMap.Values);

            if (previewTiles.Count == 0) return;

            centerPreviewTile = previewTiles[GetCenterTileIndex(building.Size)];
            previewOffsetRoot.localPosition = -tilesTransform.InverseTransformPoint(centerPreviewTile.transform.position);
            OnProductConfirmed?.Invoke(ClearPreview);
        }

        public override void OnTileHovered(GridTile tile)
        {
            lastHoveredTile = tile;
            tilesTransform.position = new Vector3(tile.transform.position.x, tilesTransform.position.y, tile.transform.position.z);

            hoveredTiles.Clear();
            foreach (var previewTile in previewTiles)
                hoveredTiles.Add(GridManager.GetTileAtPosition(previewTile.transform.position));

            HighlightTiles(IsPlacementValid());
            UpdatePreview();
        }

        protected override void UpdatePreview()
        {
            if (hoveredTiles.Count == 0) return;

            tilesTransform.gameObject.SetActive(true);
            CreateOrUpdatePreviewInstance(currentBuilding, GetCenterOfPreviewTiles());
        }

        public override void OnConfirmPlacement()
        {
            if (!IsPlacementValid()) return;

            foreach (var tile in hoveredTiles)
            {
                tile.Occupied = true;
                tile.Product = currentBuilding;
            }

            SetTilesColor(currentBuilding.OccupiedGradient.Evaluate(Random.Range(0f, 1f)));

            var building = GameLogicMediator.BuildingFactory.CreateBuilding(currentBuilding, GetCenterOfPreviewTiles());
            building.transform.rotation = Quaternion.Euler(0f, rotationStep * RotationIncrement, 0f);
            ClearPreview();

            GameStateManager.SetState(Enums.GameStateType.Idle);
            OnPlacementConfirmed?.Invoke();
        }

        public override void ClearPreview()
        {
            base.ClearPreview();
            currentBuilding = null;
        }
    }
}
