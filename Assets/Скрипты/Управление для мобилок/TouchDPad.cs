using UnityEngine;

public class TouchDPad : MonoBehaviour
{
    public PlayerWarp player;

    public void SetDirection(Vector2 dir) =>
        player.player.data.movement.dir = dir;

    public void ResetDirection() =>
        player.player.data.movement.dir = Vector2.zero;
}
