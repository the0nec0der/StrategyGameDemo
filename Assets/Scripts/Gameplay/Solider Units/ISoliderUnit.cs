using System;
using System.Collections.Generic;

using Gameplay.Buildings;

using GridSystem;

namespace Gameplay.SoldierUnits
{
    public interface ISoliderUnit
    {
        float Damage { get; }
        Action OnMovementComplete { get; set; }

        void MoveAlongPath(List<GridTile> path);
        void Attack(IBuilding target);
    }
}
