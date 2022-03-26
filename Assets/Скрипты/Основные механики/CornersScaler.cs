using UnityEngine;

public class CornersScaler : MonoBehaviour
{
    Canvas canvas;

    void Awake() =>
        canvas = GetComponent<Canvas>();

    void Update() 
    {
        Vector2Int scaler = new Vector2Int(Mathf.CeilToInt(Screen.width / 608), Mathf.CeilToInt(Screen.height / 416));
        canvas.scaleFactor = scaler.x == scaler.y ? scaler.x : scaler.x < scaler.y ? scaler.x : scaler.y;
    }
}

