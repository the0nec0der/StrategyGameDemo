using System.Collections.Generic;

using Gameplay;
using Gameplay.SoldierUnits;

using GridSystem;

using UnityEngine;

namespace PlacingSystem
{
    public class SoldierPlacer : BasePlacer<ISoliderUnitData>
    {
        private ISoliderUnitData currentSoldier;

        public void StartPlacingSoldier(ISoliderUnitData soldier)
        {
            GameStateManager.SetState(Enums.GameStateType.SoldierPlacement);
            ClearPreview();

            currentSoldier = soldier;
            previewTileMap = GridManager.GenerateGrid(previewOffsetRoot, 1, 1, isPreview: true);
            previewTiles = new List<GridTile>(previewTileMap.Values);

            if (previewTiles.Count == 0) return;

            centerPreviewTile = previewTiles[0];
            previewOffsetRoot.localPosition = -tilesTransform.InverseTransformPoint(centerPreviewTile.transform.position);
            OnProductConfirmed?.Invoke(ClearPreview);
        }

        public override void OnTileHovered(GridTile tile)
        {
            lastHoveredTile = tile;
            tilesTransform.position = new Vector3(tile.transform.position.x, tilesTransform.position.y, tile.transform.position.z);

            hoveredTiles.Clear();
            hoveredTiles.Add(GridManager.GetTileAtPosition(tile.transform.position));

            HighlightTiles(IsPlacementValid());
            UpdatePreview();
        }

        protected override void UpdatePreview()
        {
            if (hoveredTiles.Count == 0) return;

            tilesTransform.gameObject.SetActive(true);
            CreateOrUpdatePreviewInstance(currentSoldier, hoveredTiles[0].transform.position);
        }

        public override void OnConfirmPlacement()
        {
            if (!IsPlacementValid()) return;

            var tile = hoveredTiles[0];
            tile.Occupied = true;

            SeTilesColor(currentSoldier.OccupiedGradient.Evaluate(Random.Range(0f, 1f)));

            var soldierUnit = GameLogicMediator.SoldierFactory.CreateSoldier(currentSoldier, tile.transform.position);
            soldierUnit.transform.rotation = Quaternion.Euler(0f, rotationStep * RotationIncrement, 0f);

            tile.RuntimeSoldier = soldierUnit;
            tile.Product = currentSoldier;

            ClearPreview();

            GameStateManager.SetState(Enums.GameStateType.Idle);
            OnPlacementConfirmed?.Invoke();
        }

        public override void ClearPreview()
        {
            base.ClearPreview();
            currentSoldier = null;
        }
    }
}
