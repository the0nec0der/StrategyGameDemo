using System.Collections.Generic;

using Enums;

using UnityEngine;

namespace Gameplay.StatSystem
{
    [System.Serializable]
    public class GridRangePattern
    {
        [SerializeField] private int minRange = 1;
        [SerializeField] private int maxRange = 1;
        [SerializeField] private List<Vector2Int> excludedOffsets = new();

        public int MinRange => minRange;
        public int MaxRange => maxRange;

        public IEnumerable<Vector2Int> GetAllRelativePositions(GridLayoutType layout)
        {
            List<Vector2Int> positions = new();

            switch (layout)
            {
                case GridLayoutType.Square:
                    for (int x = -maxRange; x <= maxRange; x++)
                    {
                        for (int y = -maxRange; y <= maxRange; y++)
                        {
                            Vector2Int offset = new(x, y);
                            int dist = Mathf.Abs(x) + Mathf.Abs(y);

                            if (dist >= minRange && dist <= maxRange && offset != Vector2Int.zero && !excludedOffsets.Contains(offset))
                                positions.Add(offset);
                        }
                    }
                    break;

                case GridLayoutType.HexFlatTop:
                case GridLayoutType.HexPointyTop:
                    for (int q = -maxRange; q <= maxRange; q++)
                    {
                        for (int r = Mathf.Max(-maxRange, -q - maxRange); r <= Mathf.Min(maxRange, -q + maxRange); r++)
                        {
                            int s = -q - r;
                            int dist = (Mathf.Abs(q) + Mathf.Abs(r) + Mathf.Abs(s)) / 2;

                            if (dist >= minRange && dist <= maxRange)
                                positions.Add(new Vector2Int(q, r));
                        }
                    }
                    break;
            }

            return positions;
        }

        public bool IsInRange(Vector2Int from, Vector2Int to, GridLayoutType layout)
        {
            Vector2Int delta = to - from;

            switch (layout)
            {
                case GridLayoutType.Square:
                    int manhattan = Mathf.Abs(delta.x) + Mathf.Abs(delta.y);
                    return manhattan >= minRange && manhattan <= maxRange && !excludedOffsets.Contains(delta);

                case GridLayoutType.HexFlatTop:
                case GridLayoutType.HexPointyTop:
                    int dist = (Mathf.Abs(delta.x) + Mathf.Abs(delta.y) + Mathf.Abs(-delta.x - delta.y)) / 2;
                    return dist >= minRange && dist <= maxRange;
            }

            return false;
        }
    }
}
