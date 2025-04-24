using System;

using UnityEngine;

namespace GridSystem
{
    [Serializable]
    public class TileColorData : ITileColor
    {
        [SerializeField] private Gradient occupiedGradient;

        public Gradient OccupiedGradient => occupiedGradient;
    }
}
