using System.Collections.Generic;

using UnityEngine;

namespace GridSystem
{
    [CreateAssetMenu(fileName = "Hex Flat Top Grid", menuName = "Game/Grid Layout/Hex Flat Top Grid Layout")]
    public class HexFlatTopGridLayout : GridLayoutAsset
    {
        [SerializeField, Range(1, 50)] private int _gridWidth = 16;
        [SerializeField, Range(1, 50)] private int _gridHeight = 9;
        [SerializeField] private float hexSize = 1f;

        public override Dictionary<Vector2, NodeBase> GenerateGrid()
        {
            var tiles = new Dictionary<Vector2, NodeBase>();
            var gridParent = new GameObject("Hex Flat Top Grid");

            float width = hexSize * 2f;
            float height = Mathf.Sqrt(3) * hexSize;

            for (int row = 0; row < _gridHeight; row++)
            {
                for (int col = 0; col < _gridWidth; col++)
                {
                    float x = col * (3f / 4f * width);
                    float y = row * height;

                    if (col % 2 == 1)
                        y += height / 2f;

                    Vector2 position = new Vector2(x, y);
                    var tile = Instantiate(nodeBasePrefab, gridParent.transform);
                    tile.Init(DecideIfObstacle(), new HexFlatCoords(position));
                    tiles.Add(position, tile);
                }
            }

            return tiles;
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
}
