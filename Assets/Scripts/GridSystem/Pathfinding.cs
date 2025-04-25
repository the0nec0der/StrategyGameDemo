using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GridSystem
{
    public static class Pathfinding
    {
        private static readonly Color PathColor = new Color(0.65f, 0.35f, 0.35f);

        public static List<GridTile> FindPath(GridTile startNode, GridTile targetNode)
        {
            var toSearch = new List<GridTile>() { startNode };
            var processed = new HashSet<GridTile>();

            foreach (var tile in startNode.Neighbors)
            {
                tile.SetConnection(null);
                tile.SetG(float.MaxValue);
                tile.SetH(0);
            }

            startNode.SetG(0);
            startNode.SetH(startNode.GetDistance(targetNode));

            while (toSearch.Any())
            {
                var current = toSearch.OrderBy(t => t.F).ThenBy(t => t.H).First();

                if (current == targetNode)
                {
                    var path = new List<GridTile>();
                    var currentPathTile = targetNode;

                    int safetyCount = 100;
                    while (currentPathTile != startNode)
                    {
                        path.Add(currentPathTile);
                        currentPathTile = currentPathTile.Connection;
                        safetyCount--;
                        if (safetyCount < 0) throw new Exception("Infinite loop in path tracing");
                    }

                    path.Reverse();
                    foreach (var tile in path) tile.SetColor(PathColor);
                    startNode.SetColor(PathColor);

                    return path;
                }

                toSearch.Remove(current);
                processed.Add(current);

                foreach (var neighbor in current.Neighbors.Where(t => t.Walkable && !t.Occupied))
                {
                    if (processed.Contains(neighbor))
                        continue;

                    float tentativeG = current.G + current.GetDistance(neighbor);

                    if (!toSearch.Contains(neighbor))
                    {
                        neighbor.SetConnection(current);
                        neighbor.SetG(tentativeG);
                        neighbor.SetH(neighbor.GetDistance(targetNode));
                        toSearch.Add(neighbor);
                    }
                    else if (tentativeG < neighbor.G)
                    {
                        neighbor.SetConnection(current);
                        neighbor.SetG(tentativeG);
                    }
                }
            }

            return null;
        }
    }
}
