using UnityEngine;

namespace GridSystem
{
    public struct HexPointyCoordinates : ICoordinates
    {
        private readonly int q;
        private readonly int r;

        public HexPointyCoordinates(int q, int r)
        {
            this.q = q;
            this.r = r;
            Pos = this.q * new Vector2(Sqrt3, 0) / 2 + this.r * new Vector2(Sqrt3 / 2, 1.5f) / 2;
        }

        public float GetDistance(ICoordinates other) => (this - (HexPointyCoordinates)other).AxialLength();

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

        public static HexPointyCoordinates operator -(HexPointyCoordinates a, HexPointyCoordinates b)
        {
            return new HexPointyCoordinates(a.q - b.q, a.r - b.r);
        }
    }
}