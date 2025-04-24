using Gameplay.SoldierUnits;

using GridSystem;

using UnityEngine;

namespace PlacingSystem
{
    public class SoldierPlacer : BasePlacer<ISoliderUnit>
    {
        private ISoliderUnit currentSoldier;
        private GameObject previewInstance;

        public void StartPlacingSoldier(ISoliderUnit soldier)
        {
            currentSoldier = soldier;
            tilesTransform.gameObject.SetActive(true);
        }

        public override void OnTileHovered(GridTile tile)
        {
            lastHoveredTile = tile;
            hoveredTiles.Clear();
            hoveredTiles.Add(tile);
            HighlightTiles(IsPlacementValid());
            UpdatePreview();
        }

        protected override void UpdatePreview()
        {
            if (hoveredTiles.Count == 0) return;

            var tile = hoveredTiles[0];
            if (previewInstance == null)
            {
                previewInstance = Instantiate(currentSoldier.Prefab, tile.transform.position, Quaternion.Euler(0f, rotationStep * RotationIncrement, 0f), tilesTransform);
                DisablePreviewBehavior(previewInstance);
            }
            else
            {
                previewInstance.transform.position = tile.transform.position;
                previewInstance.transform.rotation = Quaternion.Euler(0f, rotationStep * RotationIncrement, 0f);
            }
        }

        public override void OnConfirmPlacement()
        {
            if (!IsPlacementValid()) return;

            var tile = hoveredTiles[0];
            tile.Occupied = true;
            GameLogicMediator.Instance.SoldierFactory.CreateSoldier(currentSoldier, tile.transform.position);
            ClearPreview();
        }

        public override void ClearPreview()
        {
            if (previewInstance != null)
            {
                Destroy(previewInstance);
                previewInstance = null;
            }

            currentSoldier = null;
            hoveredTiles.Clear();
            tilesTransform.rotation = Quaternion.identity;
            rotationStep = 0;
            tilesTransform.gameObject.SetActive(false);
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
