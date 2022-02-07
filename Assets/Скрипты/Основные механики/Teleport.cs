using UnityEngine;

public class Teleport : MonoBehaviour 
{
    public Transform teleportTo;

    public void TeleportTo(GameObject player) => 
        player.transform.position = teleportTo.position;
}
