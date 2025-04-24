using Core.InstanceSystem;

using Enums;

using System;
using UnityEngine;

namespace Gameplay
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance => Instanced<GameStateManager>.Instance;

        public GameStateType CurrentState { get; private set; } = GameStateType.Idle;

        public event Action<GameStateType> OnGameStateChanged;

        private GameStateType? previousState = null;

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

            ExitState(CurrentState);
            CurrentState = newState;
            EnterState(CurrentState);

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
                    GameLogicMediator.Instance.BuildingPlacer.enabled = true;
                    break;
                case GameStateType.SoldierPlacement:
                    GameLogicMediator.Instance.SoldierPlacer.enabled = true;
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
                    GameLogicMediator.Instance.BuildingPlacer.enabled = false;
                    break;
                case GameStateType.SoldierPlacement:
                    GameLogicMediator.Instance.SoldierPlacer.enabled = false;
                    break;
                    // Add cleanup for other states if needed
            }
        }

        public bool IsState(GameStateType state) => CurrentState == state;
    }
}
