using UnityEngine;

namespace GridSystem
{
    public struct HexFlatCoordinates : ICoordinates
    {
        private readonly int q;
        private readonly int r;

        public HexFlatCoordinates(int q, int r)
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

        public float GetDistance(ICoordinates other)
        {
            var a = OffsetToCube(q, r);
            var b = OffsetToCube(((HexFlatCoordinates)other).q, ((HexFlatCoordinates)other).r);
            return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z)) / 2f;
        }

        private (int x, int y, int z) OffsetToCube(int q, int r)
        {
            int x = q;
            int z = r - (q - (q & 1)) / 2;
            int y = -x - z;
            return (x, y, z);
        }


        public static HexFlatCoordinates operator -(HexFlatCoordinates a, HexFlatCoordinates b)
        {
            return new HexFlatCoordinates(a.q - b.q, a.r - b.r);
        }
    }
}
