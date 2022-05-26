using UnityEngine;

public class TouchDPadButton : MonoBehaviour
{
    public TouchDPad touchDPad;
    public Vector2 direction;

    public void Press() =>
        touchDPad.SetDirection(direction);
}
