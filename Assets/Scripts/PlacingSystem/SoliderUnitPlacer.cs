using System.Collections.Generic;

using Gameplay;
using Gameplay.SoldierUnits;

using GridSystem;

using UnityEngine;

namespace PlacingSystem
{
    public class SoldierPlacer : BasePlacer<ISoliderUnit>
    {
        private ISoliderUnit currentSoldier;

        private GameStateManager GameStateManager => GameStateManager.Instance;
        private GameLogicMediator GameLogicMediator => GameLogicMediator.Instance;
        private GridManager GridManager => GridManager.Instance;

        public void StartPlacingSoldier(ISoliderUnit soldier)
        {
            GameStateManager.SetState(Enums.GameStateType.SoldierPlacement);

            ClearPreview();

            currentSoldier = soldier;
            previewTileMap = GridManager.GenerateGrid(previewOffsetRoot, 1, 1, isPreview: true);
            previewTiles = new List<GridTile>(previewTileMap.Values);

            if (previewTiles.Count == 0) return;

            centerPreviewTile = previewTiles[0]; // Only one tile used
            Vector3 localCenter = tilesTransform.InverseTransformPoint(centerPreviewTile.transform.position);
            previewOffsetRoot.localPosition = -localCenter;

            OnProductConfirmed?.Invoke(ClearPreview);
        }

        public override void OnTileHovered(GridTile tile)
        {
            lastHoveredTile = tile;
            tilesTransform.position = new Vector3(tile.transform.position.x, tilesTransform.position.y, tile.transform.position.z);

            hoveredTiles.Clear();
            var gridTile = GridManager.GetTileAtPosition(tile.transform.position);
            hoveredTiles.Add(gridTile);

            HighlightTiles(IsPlacementValid());
            UpdatePreview();
        }

        protected override void UpdatePreview()
        {
            if (hoveredTiles.Count == 0) return;

            tilesTransform.gameObject.SetActive(true);

            Vector3 center = hoveredTiles[0].transform.position;

            if (productPreviewInstance == null)
            {
                productPreviewInstance = Instantiate(currentSoldier.Prefab, center, Quaternion.Euler(0f, rotationStep * RotationIncrement, 0f), tilesTransform);
                DisablePreviewBehavior(productPreviewInstance);
            }

            productPreviewInstance.transform.position = center;
            productPreviewInstance.transform.rotation = Quaternion.Euler(0f, rotationStep * RotationIncrement, 0f);

            foreach (var renderer in productPreviewInstance.GetComponentsInChildren<Renderer>())
                renderer.material = productPreviewMaterialTransparent;
        }

        public override void OnConfirmPlacement()
        {
            if (!IsPlacementValid()) return;

            var tile = hoveredTiles[0];
            tile.Occupied = true;
            tile.Product = currentSoldier;

            SeTilesColor(currentSoldier.OccupiedGradient.Evaluate(Random.Range(0f, 1f)));

            var soliderUnit = GameLogicMediator.SoldierFactory.CreateSoldier(currentSoldier, tile.transform.position);
            soliderUnit.transform.rotation = Quaternion.Euler(0f, rotationStep * RotationIncrement, 0f);
            ClearPreview();

            GameStateManager.SetState(Enums.GameStateType.Idle);
            OnPlacementConfirmed?.Invoke();
        }

        public override void ClearPreview()
        {
            base.ClearPreview();
            currentSoldier = null;
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
