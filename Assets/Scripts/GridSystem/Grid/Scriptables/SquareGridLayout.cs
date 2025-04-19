using System.Collections.Generic;

using UnityEngine;

namespace GridSystem
{
    [CreateAssetMenu(fileName = "Square Grid Layout", menuName = "Game/Grid Layout/Square Grid Layout")]
    public class SquareGridLayout : GridLayoutAsset
    {
        [SerializeField, Range(3, 50)] private int _gridWidth = 16;
        [SerializeField, Range(3, 50)] private int _gridHeight = 9;

        public override Dictionary<Vector2, NodeBase> GenerateGrid()
        {
            var tiles = new Dictionary<Vector2, NodeBase>();
            var gridParent = new GameObject("Hex Pointy Top Grid");

            for (int x = 0; x < _gridWidth; x++)
            {
                for (int y = 0; y < _gridHeight; y++)
                {
                    var tile = Instantiate(nodeBasePrefab, gridParent.transform);
                    tile.Init(DecideIfObstacle(), new SquareCoords { Pos = new Vector3(x, y) });
                    tiles.Add(new Vector2(x, y), tile);
                }
            }

            return tiles;
        }
    }
}
