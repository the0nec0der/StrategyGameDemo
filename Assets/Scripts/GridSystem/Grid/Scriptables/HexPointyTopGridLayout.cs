using System.Collections.Generic;

using UnityEngine;

namespace GridSystem
{
    [CreateAssetMenu(fileName = "Hex Pointy Top Grid", menuName = "Game/Grid Layout/Hex Pointy Top Grid Layout")]
    public class HexPointyTopGridLayout : GridLayoutAsset
    {
        [SerializeField, Range(1, 50)] private int _gridWidth = 16;
        [SerializeField, Range(1, 50)] private int _gridDepth = 9;

        public override Dictionary<Vector2, NodeBase> GenerateGrid()
        {
            var tiles = new Dictionary<Vector2, NodeBase>();
            var gridParent = new GameObject("Hex Pointy Top Grid");

            for (var r = 0; r < _gridDepth; r++)
            {
                int rOffset = r >> 1;
                for (var q = -rOffset; q < _gridWidth - rOffset; q++)
                {
                    var tile = Instantiate(nodeBasePrefab, gridParent.transform);
                    tile.Init(DecideIfObstacle(), new HexPointyCoords(q, r));
                    tiles.Add(tile.Coords.Pos, tile);
                }
            }

            return tiles;
        }
    }

    public struct HexPointyCoords : ICoords
    {
        private readonly int _q;
        private readonly int _r;

        public HexPointyCoords(int q, int r)
        {
            _q = q;
            _r = r;
            Pos = _q * new Vector2(Sqrt3, 0) / 2 + _r * new Vector2(Sqrt3 / 2, 1.5f) / 2;
        }

        public float GetDistance(ICoords other) => (this - (HexPointyCoords)other).AxialLength();

        private static readonly float Sqrt3 = Mathf.Sqrt(3);

        public Vector2 Pos { get; set; }

        private int AxialLength()
        {
            if (_q == 0 && _r == 0) return 0;
            if (_q > 0 && _r >= 0) return _q + _r;
            if (_q <= 0 && _r > 0) return -_q < _r ? _r : -_q;
            if (_q < 0) return -_q - _r;
            return -_r > _q ? -_r : _q;
        }

        public static HexPointyCoords operator -(HexPointyCoords a, HexPointyCoords b)
        {
            return new HexPointyCoords(a._q - b._q, a._r - b._r);
        }
    }
}
