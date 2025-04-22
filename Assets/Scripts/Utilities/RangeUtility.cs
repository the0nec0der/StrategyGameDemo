using Enums;

using Gameplay.StatSystem;

using UnityEngine;

namespace Utilities
{
    public static class RangeUtility
    {
        public static bool CanAttack(Vector2Int attackerPos, Vector2Int targetPos, IRangeStat rangeStat, GridLayoutType layout)
        {
            return rangeStat.AttackPattern.IsInRange(attackerPos, targetPos, layout);
        }
    }
}
