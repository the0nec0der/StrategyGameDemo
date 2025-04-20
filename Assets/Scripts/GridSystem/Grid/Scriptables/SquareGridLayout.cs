using System.Collections.Generic;

using Enums;

using UnityEngine;

namespace GridSystem
{
    [CreateAssetMenu(fileName = "Square Grid Layout", menuName = "Game/Grid Layout/Square Grid Layout")]
    public class SquareGridLayout : GridLayoutAsset
    {
        public override Dictionary<Vector2, NodeBase> GenerateGrid(int tileSize, GridOrientation orientation)
        {
            var tiles = new Dictionary<Vector2, NodeBase>();
            var gridParent = new GameObject("Hex Pointy Top Grid");

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    var tile = Instantiate(nodeBasePrefab, gridParent.transform);
                    tile.Init(DecideIfObstacle(), new SquareCoords { Pos = new Vector3(x, y) });
                    tiles.Add(new Vector2(x, y), tile);
                }
            }

            gridParent.transform.localScale = Vector3.one * tileSize;
            gridParent.transform.rotation = orientation == GridOrientation.Horizontal
                ? Quaternion.Euler(90f, 0f, 0f)
                : Quaternion.identity;

            return tiles;
        }
    }
}
