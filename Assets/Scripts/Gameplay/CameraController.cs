using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Gameplay
{
    public class CameraController : MonoBehaviour, GameInputActions.ICameraActions
    {
        [Header("References")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Transform targetPoint;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float dragSpeed = 1f;
        [SerializeField] private float baseZoom = 10f;

        [Header("Rotation")]
        [SerializeField] private float rotationSpeed = 0.2f;

        [Header("Zoom")]
        [SerializeField] private float zoomSpeed = 5f;
        [SerializeField] private float minZoom = 5f;
        [SerializeField] private float maxZoom = 25f;

        private GameInputActions controls;
        private Vector2 movementInput;
        private Vector2 zoomInput;
        private Camera mainCamera;

        private bool isRightClickHeld = false;
        private bool isMiddleClickHeld = false;

        private void Awake()
        {
            mainCamera = cameraTransform.GetComponent<Camera>();
            controls = new GameInputActions();
            controls.Camera.SetCallbacks(this);
        }

        private void OnEnable() => controls.Camera.Enable();
        private void OnDisable() => controls.Camera.Disable();

        private void Update()
        {
            if (GameStateManager.Instance.IsState(Enums.GameStateType.UI))
                return;

            if (IsPointerOverUI())
                return;

            HandleKeyboardMovement();
            HandleMouseDrag();
            HandleRotation();
            HandleZoom();
        }

        private void HandleKeyboardMovement()
        {
            if (movementInput == Vector2.zero) return;

            float yRotation = transform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, yRotation, 0f);

            Vector3 inputDir = new Vector3(movementInput.x, 0f, movementInput.y);
            Vector3 worldDirection = rotation * inputDir;

            float zoomFactor = mainCamera.orthographicSize / baseZoom;
            transform.position += worldDirection * moveSpeed * zoomFactor * Time.deltaTime;
        }

        private void HandleMouseDrag()
        {
            if (!isRightClickHeld) return;

            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            Vector3 inputDir = new Vector3(-mouseDelta.x, 0f, -mouseDelta.y);

            float yRotation = transform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, yRotation, 0f);
            Vector3 worldDirection = rotation * inputDir;

            float zoomFactor = mainCamera.orthographicSize / baseZoom;
            transform.position += worldDirection * dragSpeed * zoomFactor * Time.deltaTime;
        }

        private void HandleRotation()
        {
            if (!isMiddleClickHeld) return;

            float deltaX = Mouse.current.delta.ReadValue().x;
            transform.RotateAround(targetPoint.position, Vector3.up, deltaX * rotationSpeed);
        }

        private void HandleZoom()
        {
            float zoomDelta = -zoomInput.y * zoomSpeed * Time.deltaTime;
            float newSize = Mathf.Clamp(mainCamera.orthographicSize + zoomDelta, minZoom, maxZoom);
            mainCamera.orthographicSize = newSize;
        }

        public void OnMoveCamera(InputAction.CallbackContext context)
        {
            movementInput = context.ReadValue<Vector2>();
        }

        public void OnRotateCamera(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                isRightClickHeld = Mouse.current.rightButton.isPressed;
                isMiddleClickHeld = Mouse.current.middleButton.isPressed;
            }

            if (context.canceled)
            {
                isRightClickHeld = false;
                isMiddleClickHeld = false;
            }
        }

        public void OnZoomCamera(InputAction.CallbackContext context)
        {
            zoomInput = context.ReadValue<Vector2>();
        }

        public void CenterCamera(Vector3 centerPoint)
        {
            transform.position = new Vector3(centerPoint.x, transform.position.y, centerPoint.z);
        }

        private bool IsPointerOverUI()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }
    }
}
