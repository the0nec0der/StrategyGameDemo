using System.Collections.Generic;

using UnityEngine;

namespace GridSystem
{
    [CreateAssetMenu(fileName = "New Grid Tile Group Shape", menuName = "Grid/Grid Tile Group Shape")]
    public class GridTileGroupShapeAsset : ScriptableObject
    {
        [SerializeField] private string shapeName;
        [SerializeField] private List<Vector2Int> offsets = new List<Vector2Int> { Vector2Int.zero };
        [SerializeField] private GameObject groupPrefab;

        public string ShapeName => shapeName;
        public List<Vector2Int> Offsets => offsets;
        public GameObject GroupPrefab => groupPrefab;

        public GridTileGroupShape GetShape()
        {
            return new GridTileGroupShape(offsets.ToArray());
        }
    }
}
