using UnityEngine;

namespace Gameplay
{
    public class CameraController : MonoBehaviour
    {
        [Header("Zoom")]
        [SerializeField] private float zoomSpeed = 10f;
        [SerializeField] private float minZoom = 2f;
        [SerializeField] private float maxZoom = 8f;

        private Camera cam = null;
        private Vector3 lastMousePosition = Vector3.zero;

        private void Awake()
        {
            cam = GetComponent<Camera>();
        }

        private void CenterCameraOnGrid(Vector2 center)
        {
            cam.transform.position = new Vector3(center.x, center.y, cam.transform.position.z);
        }

        private void Update()
        {
            HandleZoom();
            HandlePan();
        }

        private void HandleZoom()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);
        }

        private void HandlePan()
        {
            if (Input.GetMouseButtonDown(1))
                lastMousePosition = Input.mousePosition;

            if (Input.GetMouseButton(1))
            {
                Vector3 delta = cam.ScreenToWorldPoint(lastMousePosition) - cam.ScreenToWorldPoint(Input.mousePosition);
                cam.transform.position += delta;
                lastMousePosition = Input.mousePosition;
            }
        }
    }
}
