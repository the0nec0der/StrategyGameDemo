using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GridSystem
{
    public class HexFlatTopNode : NodeBase
    {
        public override void CacheNeighbors()
        {
            Neighbors = GridManager.Instance.Tiles
                .Where(t => Coords.GetDistance(t.Value.Coords) == 1)
                .Select(t => t.Value)
                .ToList();
        }
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            foreach (var tile in GridManager.Instance.Tiles)
            {
                Gizmos.DrawWireSphere(tile.Key, 0.2f);
            }
        }
    }
    public struct HexFlatCoords : ICoords
    {
        private readonly int q;
        private readonly int r;

        public HexFlatCoords(int q, int r)
        {
            this.q = q;
            this.r = r;

            float hexWidth = 1f;
            float hexHeight = Mathf.Sqrt(3) / 2f * hexWidth;

            float x = hexWidth * 0.75f * q;
            float y = hexHeight * (r + 0.5f * (q & 1));

            Pos = new Vector2(x, y);
        }

        public Vector2 Pos { get; set; }

        public float GetDistance(ICoords other)
        {
            var a = OffsetToCube(q, r);
            var b = OffsetToCube(((HexFlatCoords)other).q, ((HexFlatCoords)other).r);
            return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z)) / 2f;
        }

        private (int x, int y, int z) OffsetToCube(int q, int r)
        {
            int x = q;
            int z = r - (q - (q & 1)) / 2;
            int y = -x - z;
            return (x, y, z);
        }


        public static HexFlatCoords operator -(HexFlatCoords a, HexFlatCoords b)
        {
            return new HexFlatCoords(a.q - b.q, a.r - b.r);
        }
    }
}
