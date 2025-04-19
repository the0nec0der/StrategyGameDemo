using System.Collections.Generic;

using UnityEngine;

namespace GridSystem
{
    [CreateAssetMenu(fileName = "Hex Flat Top Grid", menuName = "Game/Grid Layout/Hex Flat Top Grid Layout")]
    public class HexFlatTopGridLayout : GridLayoutAsset
    {
        public override Dictionary<Vector2, NodeBase> GenerateGrid()
        {
            var tiles = new Dictionary<Vector2, NodeBase>();
            var gridParent = new GameObject("Hex Flat Top Grid");

            float width = 2f;
            float height = Mathf.Sqrt(3);

            for (int r = 0; r < gridHeight; r++)
            {
                for (int q = 0; q < gridWidth; q++)
                {
                    float x = q * (3f / 4f * width);
                    float y = r * height;

                    if (q % 2 == 1)
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
}
