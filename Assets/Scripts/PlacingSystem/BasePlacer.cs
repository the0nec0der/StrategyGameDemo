using System;
using System.Collections.Generic;

using Enums;
using Gameplay;
using Gameplay.Product;

using GridSystem;

using UnityEngine;

namespace PlacingSystem
{
    public abstract class BasePlacer<T> : MonoBehaviour where T : class, IProduct
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

        public Action OnPlacementConfirmed;
        public Action<Action> OnProductConfirmed;

        protected int RotationIncrement => GridManager.Instance.LayoutType switch
        {
            GridLayoutType.Square => 90,
            GridLayoutType.HexFlatTop => 60,
            GridLayoutType.HexPointyTop => 60,
            _ => 90
        };

        protected GameStateManager GameStateManager => GameStateManager.Instance;
        protected GameLogicMediator GameLogicMediator => GameLogicMediator.Instance;
        protected GridManager GridManager => GridManager.Instance;

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

        protected virtual void OnDisable() => inputActions?.Placement.Disable();

        public virtual void ClearPreview()
        {
            if (productPreviewInstance != null)
                Destroy(productPreviewInstance);

            if (previewOffsetRoot != null)
                Destroy(previewOffsetRoot.gameObject);

            previewOffsetRoot = new GameObject("OffsetRoot").transform;
            previewOffsetRoot.SetParent(tilesTransform, false);

            tilesTransform.rotation = Quaternion.identity;
            rotationStep = 0;

            productPreviewInstance = null;
            previewTiles.Clear();
            hoveredTiles.Clear();
            previewTileMap.Clear();
            tilesTransform.gameObject.SetActive(false);
        }

        protected virtual void DisablePreviewBehavior(GameObject instance)
        {
            foreach (var col in instance.GetComponentsInChildren<Collider>())
                col.enabled = false;

            foreach (var mono in instance.GetComponentsInChildren<MonoBehaviour>())
                mono.enabled = false;
        }

        public abstract void OnConfirmPlacement();
        public abstract void OnTileHovered(GridTile tile);
        protected abstract void UpdatePreview();

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
            foreach (var tile in previewTiles)
            {
                if (tile == null) continue;
                var renderer = tile.GetComponentInChildren<SpriteRenderer>();
                if (renderer != null)
                    renderer.color = canPlace ? tilePreviewMaterialGreen : tilePreviewMaterialRed;
            }
        }

        protected void SetTilesColor(Color32 color)
        {
            foreach (var tile in hoveredTiles)
            {
                tile.ChangedColor = color;
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

            return center / previewTiles.Count;
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

        protected void CreateOrUpdatePreviewInstance(T product, Vector3 position)
        {
            if (productPreviewInstance == null)
            {
                productPreviewInstance = Instantiate(product.Prefab, position, Quaternion.Euler(0f, rotationStep * RotationIncrement, 0f), tilesTransform);
                DisablePreviewBehavior(productPreviewInstance);
            }

            productPreviewInstance.transform.position = position;
            productPreviewInstance.transform.rotation = Quaternion.Euler(0f, rotationStep * RotationIncrement, 0f);

            foreach (var renderer in productPreviewInstance.GetComponentsInChildren<Renderer>())
                renderer.material = productPreviewMaterialTransparent;
        }
    }
}
