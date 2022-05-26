using UnityEngine;

public class TouchDPad : MonoBehaviour
{
    public PlayerWarp player;

    public void SetDirection(Vector2 dir) =>
        player.player.dir = dir;

    public void ResetDirection() =>
        player.player.dir = Vector2.zero;
}
