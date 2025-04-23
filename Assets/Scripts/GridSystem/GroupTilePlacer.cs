using System.Collections.Generic;

using Gameplay.Buildings;

using UnityEngine;

namespace GridSystem
{
    public class GroupTilePlacer : MonoBehaviour
    {
        [SerializeField] private Transform tilesParentTransform;

        private IBuilding building = null;
        private Dictionary<Vector2, GridTile> tiles = new();

        public void GenerateGroupTile(IBuilding buildingReference)
        {
            ClearGroupTiles();
            building = buildingReference;
            tiles = GridManager.Instance.GenerateGrid(tilesParentTransform, building.Size.x, building.Size.y, isPreview: true);
        }

        private void ClearGroupTiles()
        {
            foreach (Transform tileTransform in tilesParentTransform)
            {
                Destroy(tileTransform);
            }
            building = null;
            tiles.Clear();
        }
    }
}
