using UnityEngine;

public static class Constants
{
    /// Ширина игрового экрана
    public const int screenWight = 608;
    /// Высота игрового экрана
    public const int screenHeight = 352;

    /// Степень масштабирования игрового экрана
    public static int scaleFactor
    {
        get
        {
            Vector2Int scaler = new Vector2Int(Mathf.CeilToInt(Screen.width / Constants.screenWight), 
                Mathf.CeilToInt(Screen.height / Constants.screenHeight));
            return Mathf.Min(scaler.x, scaler.y);
        }
    }
}