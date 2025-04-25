using System.Linq;

using Enums;

using Gameplay;
using Gameplay.Buildings;
using Gameplay.SoldierUnits;

using GridSystem;

using UnityEngine;

public class SoliderUnitCommander
{
    private GridTile originTile;
    private ISoliderUnit selectedUnit;

    public void SelectUnit(ISoliderUnit unit, GridTile tile)
    {
        selectedUnit = unit;
        originTile = tile;
        GameStateManager.Instance.SetState(GameStateType.MoveCommand);
    }

    public void IssueCommand(GridTile targetTile)
    {
        if (selectedUnit == null || originTile == null) return;
        if (!targetTile.Walkable || targetTile == originTile) return;

        if (!targetTile.Occupied)
        {
            MoveUnit(targetTile);
        }
        else if (targetTile.Product is IBuilding building)
        {
            AttackTarget(building, targetTile);
        }

        Reset();
    }

    private void MoveUnit(GridTile targetTile)
    {
        var path = Pathfinding.FindPath(originTile, targetTile);
        if (path == null || path.Count == 0) return;

        selectedUnit.MoveAlongPath(path);

        // Clear old tile
        originTile.Occupied = false;
        originTile.Product = null;
        originTile.RuntimeSoldier = null;

        // Set new tile
        if (selectedUnit is SoldierController controller)
        {
            targetTile.Occupied = true;
            targetTile.Product = controller.Data;
            targetTile.RuntimeSoldier = controller;
        }

        GameStateManager.Instance.SetState(GameStateType.Idle);
    }

    private void AttackTarget(IBuilding building, GridTile buildingTile)
    {
        var path = Pathfinding.FindPath(originTile, buildingTile);
        if (path == null || path.Count < 2) return;

        var lastWalkableTile = path[^2];
        selectedUnit.MoveAlongPath(path.Take(path.Count - 1).ToList());

        selectedUnit.OnMovementComplete = () =>
        {
            selectedUnit.Attack(building);
            GameStateManager.Instance.SetState(GameStateType.Idle);
        };
    }

    public void ShowPreviewPath(GridTile hoveredTile)
    {
        if (selectedUnit == null || originTile == null || hoveredTile == originTile)
            return;

        var path = Pathfinding.FindPath(originTile, hoveredTile);
        if (path == null || path.Count == 0) return;

        Color previewColor = hoveredTile.Occupied switch
        {
            true when hoveredTile.Product is IBuilding => Color.red,
            false => Color.green,
            _ => Color.gray
        };

        foreach (var tile in path)
            tile.SetColor(previewColor);

        originTile.SetColor(previewColor);
    }

    private void Reset()
    {
        selectedUnit = null;
        originTile = null;
    }
}
