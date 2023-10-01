using UnityEngine;

[ExecuteInEditMode]
public class CanvasScaler : MonoBehaviour
{
    Canvas canvas;

    void Awake() =>
        canvas = GetComponent<Canvas>();

    void Update() =>
        canvas.scaleFactor = Constants.scaleFactor;
}

