using System.Linq;

using UnityEngine;

namespace GridSystem
{
    public class HexNode : NodeBase
    {
        public override void CacheNeighbors()
        {
            Neighbors = GridManager.Instance.Tiles.Where(t => Coords.GetDistance(t.Value.Coords) == 1).Select(t => t.Value).ToList();
        }
    }

    public struct HexFlatCoords : ICoords
    {
        public HexFlatCoords(Vector2 pos)
        {
            Pos = new Vector2(pos.x / 2, pos.y / 2);
        }

        public float GetDistance(ICoords other)
        {
            return Vector2.Distance(Pos, other.Pos);
        }

        public Vector2 Pos { get; set; }
    }

    public struct HexPointyCoords : ICoords
    {
        private readonly int q;
        private readonly int r;

        public HexPointyCoords(int q, int r)
        {
            this.q = q;
            this.r = r;
            Pos = this.q * new Vector2(Sqrt3, 0) / 2 + this.r * new Vector2(Sqrt3 / 2, 1.5f) / 2;
        }

        public float GetDistance(ICoords other) => (this - (HexPointyCoords)other).AxialLength();

        private static readonly float Sqrt3 = Mathf.Sqrt(3);

        public Vector2 Pos { get; set; }

        private int AxialLength()
        {
            if (q == 0 && r == 0) return 0;
            if (q > 0 && r >= 0) return q + r;
            if (q <= 0 && r > 0) return -q < r ? r : -q;
            if (q < 0) return -q - r;
            return -r > q ? -r : q;
        }

        public static HexPointyCoords operator -(HexPointyCoords a, HexPointyCoords b)
        {
            return new HexPointyCoords(a.q - b.q, a.r - b.r);
        }
    }
}