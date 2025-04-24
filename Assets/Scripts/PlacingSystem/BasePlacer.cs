using System.Collections.Generic;

using Enums;

using GridSystem;

using UnityEngine;

namespace PlacingSystem
{
    public abstract class BasePlacer<T> : MonoBehaviour where T : class
    {
        [SerializeField] protected Transform tilesTransform;
        [SerializeField] protected Material productPreviewMaterialTransparent;
        [SerializeField] protected Color32 tilePreviewMaterialGreen;
        [SerializeField] protected Color32 tilePreviewMaterialRed;

        protected GridTile centerPreviewTile;
        protected GridTile lastHoveredTile;
        protected List<GridTile> previewTiles = new();
        protected List<GridTile> hoveredTiles = new();
        protected Dictionary<Vector2, GridTile> previewTileMap = new();
        protected Transform previewOffsetRoot;
        protected GameObject productPreviewInstance;
        protected int rotationStep = 0;
        private GameInputActions inputActions;

        protected int RotationIncrement => GridManager.Instance.LayoutType switch
        {
            GridLayoutType.Square => 90,
            GridLayoutType.HexFlatTop => 60,
            GridLayoutType.HexPointyTop => 60,
            _ => 90
        };

        protected virtual void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new GameInputActions();
                inputActions.Placement.RotateLeft.performed += ctx => RotatePreviewStep(-1);
                inputActions.Placement.RotateRight.performed += ctx => RotatePreviewStep(1);
            }

            inputActions.Placement.Enable();
        }

        protected virtual void OnDisable()
        {
            inputActions?.Placement.Disable();
        }

        protected virtual void ClearPreview()
        {
            if (productPreviewInstance != null)
            {
                Destroy(productPreviewInstance);
                productPreviewInstance = null;
            }

            if (previewOffsetRoot != null)
            {
                Destroy(previewOffsetRoot.gameObject);
                previewOffsetRoot = null;
            }

            previewOffsetRoot = new GameObject("OffsetRoot").transform;
            previewOffsetRoot.SetParent(tilesTransform, false);

            tilesTransform.rotation = Quaternion.identity;
            rotationStep = 0;
            previewTiles.Clear();
            hoveredTiles.Clear();
            previewTileMap.Clear();
            tilesTransform.gameObject.SetActive(false);
        }

        public virtual bool OnConfirmPlacement()
        {
            if (!IsPlacementValid()) return false;

            foreach (var tile in hoveredTiles)
                tile.Occupied = true;

            return true;
        }

        public abstract void OnTileHovered(GridTile tile);
        protected abstract void UpdatePreview();
        protected abstract void DisablePreviewBehavior(GameObject instance);

        protected void RotatePreviewStep(int direction)
        {
            if (tilesTransform == null) return;

            rotationStep += direction;
            tilesTransform.rotation = Quaternion.Euler(0f, rotationStep * RotationIncrement, 0f);

            if (lastHoveredTile != null)
                OnTileHovered(lastHoveredTile);
        }

        protected void HighlightTiles(bool canPlace)
        {
            for (int i = 0; i < previewTiles.Count; i++)
            {
                var tile = previewTiles[i];
                if (tile == null) continue;

                var spriteRenderer = tile.GetComponentInChildren<SpriteRenderer>();
                if (spriteRenderer != null)
                    spriteRenderer.color = canPlace ? tilePreviewMaterialGreen : tilePreviewMaterialRed;
            }
        }

        protected void SeTilesColor(Color32 color)
        {
            foreach (var tile in hoveredTiles)
            {
                tile.SetColor(color);
            }
        }

        protected bool IsPlacementValid()
        {
            foreach (var tile in hoveredTiles)
            {
                if (tile == null || !tile.Walkable || tile.Occupied)
                    return false;
            }
            return true;
        }

        protected Vector3 GetCenterOfPreviewTiles()
        {
            Vector3 center = Vector3.zero;
            foreach (var tile in previewTiles)
                center += tile.transform.position;

            center /= previewTiles.Count;
            return center;
        }

        protected int GetCenterTileIndex(Vector2Int size)
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
