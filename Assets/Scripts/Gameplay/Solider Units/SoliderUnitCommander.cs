using System.Collections.Generic;
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
    private List<GridTile> currentPreviewPath = new();

    public void SelectUnit(ISoliderUnit unit, GridTile tile)
    {
        // If re-clicking the same unit → deselect
        if (unit == selectedUnit && tile == originTile)
        {
            ClearPreviewPath();
            Reset();
            GameStateManager.Instance.SetState(GameStateType.Idle);
            return;
        }

        ClearPreviewPath();
        selectedUnit = unit;
        originTile = tile;

        GameStateManager.Instance.SetState(GameStateType.MoveCommand);
    }

    public void IssueCommand(GridTile targetTile)
    {
        if (selectedUnit == null || originTile == null) return;

        // Clicking the same tile cancels preview
        if (targetTile == originTile)
        {
            ClearPreviewPath();
            Reset();
            GameStateManager.Instance.SetState(GameStateType.Idle);
            return;
        }

        if (!targetTile.Walkable) return;

        if (!targetTile.Occupied)
        {
            ClearPreviewPath();
            MoveUnit(targetTile);
        }
        else if (targetTile.Product is IBuilding building)
        {
            ClearPreviewPath();
            AttackTarget(building, targetTile);
        }

        Reset();
    }

    public void ShowPreviewPath(GridTile hoveredTile)
    {
        if (selectedUnit == null || originTile == null || hoveredTile == originTile)
            return;

        ClearPreviewPath();

        var path = Pathfinding.FindPath(originTile, hoveredTile);
        if (path == null || path.Count == 0) return;

        currentPreviewPath = path;

        Color previewColor = hoveredTile.Occupied switch
        {
            true when hoveredTile.Product is IBuilding => Color.red,
            false => Color.green,
            _ => Color.gray
        };

        foreach (var tile in currentPreviewPath)
            tile.SetColor(previewColor);

        if (!originTile.Occupied)
            originTile.SetColor(previewColor);
    }

    public void ClearPreviewPath()
    {
        foreach (var tile in currentPreviewPath)
        {
            if (!tile.Occupied)
                tile.RevertTile();
        }

        if (originTile != null && !originTile.Occupied)
            originTile.RevertTile();

        currentPreviewPath.Clear();
    }

    private void MoveUnit(GridTile targetTile)
    {
        var path = Pathfinding.FindPath(originTile, targetTile);
        if (path == null || path.Count == 0) return;

        selectedUnit.MoveAlongPath(path);

        originTile.Occupied = false;
        originTile.Product = null;
        originTile.RuntimeSoldier = null;

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

    private void Reset()
    {
        originTile = null;
        selectedUnit = null;
        currentPreviewPath.Clear();
    }
}
