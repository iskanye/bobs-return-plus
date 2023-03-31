using UnityEngine;

[ExecuteInEditMode]
public class CropFrame : MonoBehaviour
{
    public Camera mainCamera;
    public Vector2Int screenSize;

    void Update()
    {
        Vector2Int scaler = new Vector2Int(Mathf.CeilToInt(Screen.width / screenSize.x), Mathf.CeilToInt(Screen.height / screenSize.y));
        float scaleFactor = scaler.x == scaler.y ? scaler.x : scaler.x < scaler.y ? scaler.x : scaler.y;
        var w = (scaleFactor * screenSize.x / Screen.width);
        var h = (scaleFactor * screenSize.y / Screen.height);
        mainCamera.rect = new Rect(.5f * (1 - w), .5f * (1 - h), w, h);
    }
}
