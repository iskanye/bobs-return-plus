using UnityEngine;

public class TouchDPad : MonoBehaviour
{
    public void SetDirection(Vector2 dir) =>
        InputManager.Active.direction = dir;

    public void ResetDirection() =>
        InputManager.Active.direction = Vector2.zero;
}
