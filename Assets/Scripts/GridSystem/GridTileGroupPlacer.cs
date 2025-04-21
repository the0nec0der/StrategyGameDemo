using System.Collections.Generic;

using Core.InstanceSystem;

using UnityEngine;

namespace GridSystem
{
    public class GridTileGroupPlacer : MonoBehaviour
    {
        public static GridTileGroupPlacer Instance => Instanced<GridTileGroupPlacer>.Instance;

        [SerializeField] private GridTileGroupShapeAsset shapeAsset;
        [SerializeField] private Color previewColorValid = Color.green;
        [SerializeField] private Color previewColorInvalid = Color.red;

        private GridTileGroupShape currentShape;
        private GameObject previewObject;
        private List<GridTile> highlightedTiles = new();

        private void Awake()
        {
            if (Instance != null)
                return;
        }

        private void Start()
        {
            if (shapeAsset != null)
            {
                SetShape(shapeAsset.GetShape());
                SetPreviewObject(Instantiate(shapeAsset.GroupPrefab, transform));
            }
        }

        public void SetShape(GridTileGroupShape shape)
        {
            currentShape = shape;
        }

        public void SetPreviewObject(GameObject preview)
        {
            previewObject = preview;
        }

        public void UpdatePreview(GridTile originTile)
        {
            ClearPreview();

            if (currentShape == null || originTile == null) return;

            bool canPlace = CanPlace(originTile, out var tiles);

            Debug.Log(canPlace);

            foreach (var tile in tiles)
            {
                tile.SetColor(canPlace ? previewColorValid : previewColorInvalid);
                highlightedTiles.Add(tile);
            }

            if (previewObject != null)
            {
                previewObject.transform.position = originTile.transform.position;
                previewObject.SetActive(true);
            }
        }

        public bool TryPlace(GridTile originTile, GameObject toPlacePrefab, out List<GridTile> occupiedTiles)
        {
            occupiedTiles = new List<GridTile>();

            if (!CanPlace(originTile, out var tiles)) return false;

            GameObject instance = Instantiate(toPlacePrefab, originTile.transform.position, Quaternion.identity);

            foreach (var tile in tiles)
            {
                tile.Occupied = false;
                occupiedTiles.Add(tile);
            }

            return true;
        }

        private bool CanPlace(GridTile originTile, out List<GridTile> resultTiles)
        {
            resultTiles = new List<GridTile>();
            Vector2Int origin = Vector2Int.RoundToInt(originTile.Coords.Pos);

            foreach (var index in currentShape.GetTileIndices(origin))
            {
                if (!GridManager.Instance.Tiles.TryGetValue(index, out var tile) || tile.Occupied || !tile.Walkable)
                    return false;

                resultTiles.Add(tile);
            }

            return true;
        }

        private void ClearPreview()
        {
            foreach (var tile in highlightedTiles)
            {
                tile.RevertTile();
            }

            highlightedTiles.Clear();

            if (previewObject != null)
                previewObject.SetActive(false);
        }
    }
}
