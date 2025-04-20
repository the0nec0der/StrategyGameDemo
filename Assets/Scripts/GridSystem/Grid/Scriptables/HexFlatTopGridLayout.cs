using System.Collections.Generic;

using UnityEngine;

namespace GridSystem
{
    [CreateAssetMenu(fileName = "Hex Flat Top Grid", menuName = "Game/Grid Layout/Hex Flat Top Grid Layout")]
    public class HexFlatTopGridLayout : GridLayoutAsset
    {
        public override Dictionary<Vector2, NodeBase> GenerateGrid(int tileSize)
        {
            var tiles = new Dictionary<Vector2, NodeBase>();
            var gridParent = new GameObject("Hex Flat Top Grid");

            for (int r = 0; r < gridHeight; r++)
            {
                for (int q = 0; q < gridWidth; q++)
                {
                    var tile = Instantiate(nodeBasePrefab, gridParent.transform);
                    tile.Init(DecideIfObstacle(), new HexFlatCoords(q, r));
                    tiles.Add(tile.Coords.Pos, tile);
                }
            }

            gridParent.transform.localScale = Vector3.one * tileSize;
            return tiles;
        }
    }
}
