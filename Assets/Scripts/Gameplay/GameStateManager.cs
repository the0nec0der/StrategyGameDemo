using System;

using Core.InstanceSystem;

using Enums;
using GridSystem;
using UnityEngine;

namespace Gameplay
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance => Instanced<GameStateManager>.Instance;

        public GameStateType CurrentState { get; private set; } = GameStateType.Idle;
        public GameStateType? PreviousState => previousState;

        public event Action<GameStateType> OnGameStateChanged;

        private GameStateType? previousState = null;

        private GameLogicMediator GameLogicMediator => GameLogicMediator.Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
        }

        public void SetState(GameStateType newState, bool storePrevious = false)
        {
            if (CurrentState == newState)
                return;

            if (storePrevious)
            {
                if (CurrentState != GameStateType.UI && CurrentState != GameStateType.Idle)
                    previousState = CurrentState;
            }
            else
            {
                previousState = null;
            }

            ExitState(CurrentState);
            CurrentState = newState;
            EnterState(CurrentState);

            Debug.Log($"Game state changed to: {CurrentState}");
            OnGameStateChanged?.Invoke(CurrentState);
        }

        public void RestorePreviousState()
        {
            if (previousState.HasValue)
            {
                SetState(previousState.Value);
                previousState = null;
            }
        }

        private void EnterState(GameStateType state)
        {
            switch (state)
            {
                case GameStateType.BuildingPlacement:
                    GameLogicMediator.BuildingPlacer.enabled = true;
                    GridManager.Instance.SoliderUnitCommander.ClearPreviewPath();
                    GameLogicMediator.SoldierPlacer.enabled = false;
                    break;
                case GameStateType.SoldierPlacement:
                    GameLogicMediator.SoldierPlacer.enabled = true;
                    break;
                case GameStateType.Selection:
                    // Enable selection visuals
                    break;
                case GameStateType.MoveCommand:
                    // Wait for right-click on grid
                    break;
                case GameStateType.AttackCommand:
                    // Wait for right-click on enemy
                    break;
                case GameStateType.UI:
                    // Block game board interaction
                    break;
            }
        }

        private void ExitState(GameStateType state)
        {
            switch (state)
            {
                case GameStateType.BuildingPlacement:
                    GameLogicMediator.BuildingPlacer.enabled = false;
                    break;
                case GameStateType.SoldierPlacement:
                    GameLogicMediator.SoldierPlacer.enabled = false;
                    break;
                    // Add cleanup for other states if needed
            }
        }

        public bool IsState(GameStateType state) => CurrentState == state;
    }
}
