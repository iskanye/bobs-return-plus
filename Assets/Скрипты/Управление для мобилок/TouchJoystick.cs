using UnityEngine;

public class TouchJoystick : MonoBehaviour
{
    public float threshold;
    public RectTransform joystick;
    public PlayerWarp player;

    Vector2 startPos;

    void Update()
    {
        var dir = Vector2.zero;

        if (Input.touchCount > 0 && player.Enabled)
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                startPos = touch.position;

            else if ((touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) &&
                (startPos - (Vector2)transform.position).sqrMagnitude < threshold * threshold)
            {
                dir = touch.position - (Vector2)transform.position;
                joystick.anchoredPosition = (touch.position - (Vector2)transform.position).sqrMagnitude > threshold * threshold ? dir.normalized * threshold : dir;
                dir = joystick.position - transform.position;
            }

            else if (touch.phase == TouchPhase.Ended)
                startPos = Vector2.zero;
        }

        else
        {
            joystick.anchoredPosition = dir;
            startPos = Vector2.zero;
        }

        player.player.data.movement.dir = dir / threshold;
    }
}
