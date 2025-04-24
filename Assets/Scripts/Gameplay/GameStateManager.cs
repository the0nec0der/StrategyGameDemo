using Core.InstanceSystem;

using Enums;

using UnityEngine;

namespace Gameplay
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance => Instanced<GameStateManager>.Instance;

        public GameStateType CurrentState { get; private set; } = GameStateType.Idle;

        private void Awake()
        {
            if (Instance != null) return;
        }

        public void SetState(GameStateType newState)
        {
            if (CurrentState == newState) return;

            ExitState(CurrentState);
            CurrentState = newState;
            EnterState(CurrentState);

            Debug.Log($"Game state changed to: {CurrentState}");
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