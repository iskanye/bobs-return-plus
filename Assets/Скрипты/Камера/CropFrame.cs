using UnityEngine;

[ExecuteInEditMode]
public class CropFrame : MonoBehaviour
{
    public Camera mainCamera;
    public float PPU;

    void Update()
    {
        float scaleFactor = Constants.scaleFactor;
        var w = scaleFactor * Constants.screenWight / Screen.width;
        var h = scaleFactor * Constants.screenHeight / Screen.height;
        mainCamera.rect = new Rect(.5f * (1 - w), .5f * (1 - h), w, h);
        mainCamera.orthographicSize = Constants.screenHeight * .5f / PPU;
    }
}
