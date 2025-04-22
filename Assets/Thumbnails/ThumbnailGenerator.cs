using System.Collections;
using System.IO;

using UnityEngine;

public class ThumbnailGenerator : MonoBehaviour
{
    [SerializeField] private RenderTexture renderTexture = null;
    [SerializeField] private string savePath = "Assets/Thumbnails/Images";

    private Texture2D texture = null;
    private Camera renderCamera = null;

    private bool renderInProgress = false;
    private void Start()
    {
        renderCamera = Camera.main;
        renderInProgress = false;

        RenderAllObjects();
    }

    [ContextMenu(nameof(RenderAllObjects))]
    private void RenderAllObjects()
    {
        texture = new(renderTexture.width, renderTexture.height, TextureFormat.RGBAFloat, false, true);
        foreach (Transform renderParent in transform)
            foreach (Transform child in renderParent)
                child.gameObject.SetActive(false);

        foreach (Transform renderParent in transform)
            foreach (Transform child in renderParent)
                StartCoroutine(GenerateThumbnail(child.gameObject, renderParent.gameObject.name));
    }

    private IEnumerator GenerateThumbnail(GameObject renderGO, string category)
    {
        while (renderInProgress)
            yield return null;

        renderInProgress = true;

        renderGO.SetActive(true);

        Bounds bounds = new();

        foreach (MeshRenderer item in renderGO.GetComponentsInChildren<MeshRenderer>())
            bounds.SetMinMax(Vector3.Min(bounds.min, item.bounds.min), Vector3.Max(bounds.max, item.bounds.max));

        renderCamera.transform.position = bounds.center;
        renderCamera.transform.position -= renderCamera.transform.forward * 50f;

        while (IsVisible(bounds) && renderCamera.orthographicSize > 2f)
            renderCamera.orthographicSize -= .01f;
        while (!IsVisible(bounds) && renderCamera.orthographicSize < 100f)
            renderCamera.orthographicSize += .01f;

        renderCamera.orthographicSize += .01f;

        yield return null;
        yield return null;

        texture = texture != null ? texture : new(renderTexture.width, renderTexture.height, TextureFormat.RGBAFloat, false, true);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();

        Directory.CreateDirectory($"{savePath}/{category}");
        File.WriteAllBytes($"{savePath}/{category}/{renderGO.name}.png", texture.EncodeToPNG());

        renderGO.SetActive(false);
        renderInProgress = false;
    }

    private bool IsVisible(Bounds bounds)
    {
        Vector3 maxLowCorner = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
        Vector3 lowMaxCorner = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        foreach (Plane plane in GeometryUtility.CalculateFrustumPlanes(renderCamera))
            if (!plane.GetSide(bounds.min) || !plane.GetSide(bounds.max) || !plane.GetSide(maxLowCorner) || !plane.GetSide(lowMaxCorner))
                return false;
        return true;
    }
}
