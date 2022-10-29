using UnityEngine;

[ExecuteInEditMode]
public class CanvasScaler : MonoBehaviour
{
    public Vector2Int screenSize;
    Canvas canvas;

    void Awake() =>
        canvas = GetComponent<Canvas>();

    void Update() 
    {
        Vector2Int scaler = new Vector2Int(Mathf.CeilToInt(Screen.width / screenSize.x), Mathf.CeilToInt(Screen.height / screenSize.y));
        canvas.scaleFactor = scaler.x == scaler.y ? scaler.x : scaler.x < scaler.y ? scaler.x : scaler.y;
    }
}

