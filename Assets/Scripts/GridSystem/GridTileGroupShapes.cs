using UnityEngine;

namespace GridSystem
{
    public static class GridTileGroupShapes
    {
        // ===== SQUARE GRID SHAPES =====
        public static GridTileGroupShape Square_1x1 => new GridTileGroupShape(
            new Vector2Int(0, 0)
        );

        public static GridTileGroupShape Square_1x2 => new GridTileGroupShape(
            new Vector2Int(0, 0), new Vector2Int(0, 1)
        );

        public static GridTileGroupShape Square_2x2 => new GridTileGroupShape(
            new Vector2Int(0, 0), new Vector2Int(0, 1),
            new Vector2Int(1, 0), new Vector2Int(1, 1)
        );

        public static GridTileGroupShape Square_2x3 => new GridTileGroupShape(
            new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2),
            new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2)
        );

        public static GridTileGroupShape Square_3x3 => new GridTileGroupShape(
            new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2),
            new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2),
            new Vector2Int(2, 0), new Vector2Int(2, 1), new Vector2Int(2, 2)
        );

        public static GridTileGroupShape Square_3x4 => new GridTileGroupShape(
            new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2), new Vector2Int(0, 3),
            new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(1, 3),
            new Vector2Int(2, 0), new Vector2Int(2, 1), new Vector2Int(2, 2), new Vector2Int(2, 3)
        );

        public static GridTileGroupShape Square_4x4 => new GridTileGroupShape(
            new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2), new Vector2Int(0, 3),
            new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(1, 3),
            new Vector2Int(2, 0), new Vector2Int(2, 1), new Vector2Int(2, 2), new Vector2Int(2, 3),
            new Vector2Int(3, 0), new Vector2Int(3, 1), new Vector2Int(3, 2), new Vector2Int(3, 3)
        );

        // ===== HEX GRID SHAPES (Same Offsets, Assumed Axial/Offset Grid) =====
        public static GridTileGroupShape Hex_1x1 => new GridTileGroupShape(
            new Vector2Int(0, 0)
        );

        public static GridTileGroupShape Hex_1x2 => new GridTileGroupShape(
            new Vector2Int(0, 0), new Vector2Int(0, 1)
        );

        public static GridTileGroupShape Hex_2x2 => new GridTileGroupShape(
            new Vector2Int(0, 0), new Vector2Int(0, 1),
            new Vector2Int(1, 0), new Vector2Int(1, 1)
        );

        public static GridTileGroupShape Hex_2x3 => new GridTileGroupShape(
            new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2),
            new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2)
        );

        public static GridTileGroupShape Hex_3x3 => new GridTileGroupShape(
            new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2),
            new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2),
            new Vector2Int(2, 0), new Vector2Int(2, 1), new Vector2Int(2, 2)
        );

        public static GridTileGroupShape Hex_3x4 => new GridTileGroupShape(
            new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2), new Vector2Int(0, 3),
            new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(1, 3),
            new Vector2Int(2, 0), new Vector2Int(2, 1), new Vector2Int(2, 2), new Vector2Int(2, 3)
        );

        public static GridTileGroupShape Hex_4x4 => new GridTileGroupShape(
            new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2), new Vector2Int(0, 3),
            new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(1, 3),
            new Vector2Int(2, 0), new Vector2Int(2, 1), new Vector2Int(2, 2), new Vector2Int(2, 3),
            new Vector2Int(3, 0), new Vector2Int(3, 1), new Vector2Int(3, 2), new Vector2Int(3, 3)
        );

        // Optional star pattern for both hex flat and pointy
        public static GridTileGroupShape Hex_Star => new GridTileGroupShape(
            new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(0, 1),
            new Vector2Int(-1, 1), new Vector2Int(-1, 0), new Vector2Int(0, -1),
            new Vector2Int(1, -1)
        );
    }
}
