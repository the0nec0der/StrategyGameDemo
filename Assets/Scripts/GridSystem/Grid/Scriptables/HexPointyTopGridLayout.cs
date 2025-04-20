using System.Collections.Generic;

using Enums;

using UnityEngine;

namespace GridSystem
{
    [CreateAssetMenu(fileName = "Hex Pointy Top Grid", menuName = "Game/Grid Layout/Hex Pointy Top Grid Layout")]
    public class HexPointyTopGridLayout : GridLayoutAsset
    {
        public override Dictionary<Vector2, NodeBase> GenerateGrid(int tileSize, GridOrientation orientation)
        {
            var tiles = new Dictionary<Vector2, NodeBase>();
            var gridParent = new GameObject("Hex Pointy Top Grid");

            for (var r = 0; r < gridHeight; r++)
            {
                int rOffset = r >> 1;
                for (var q = -rOffset; q < gridWidth - rOffset; q++)
                {
                    var tile = Instantiate(nodeBasePrefab, gridParent.transform);
                    tile.Init(DecideIfObstacle(), new HexPointyCoords(q, r));
                    tiles.Add(tile.Coords.Pos, tile);
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
