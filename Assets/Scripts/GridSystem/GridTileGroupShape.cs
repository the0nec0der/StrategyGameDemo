using System;
using System.Collections.Generic;

using UnityEngine;

namespace GridSystem
{
    [Serializable]
    public class GridTileGroupShape
    {
        [SerializeField]
        public Vector2Int[] Offsets;

        public GridTileGroupShape(params Vector2Int[] offsets)
        {
            Offsets = offsets;
        }

        public IEnumerable<Vector2Int> GetTileIndices(Vector2Int origin)
        {
            foreach (var offset in Offsets)
            {
                yield return origin + offset;
            }
        }
    }
}
