using UnityEngine;
using System.Collections;

public class Pit : MonoBehaviour
{
    public LayerMask playerMask;
    public Camera mainCamera;
    public GameObject canvas;
    public GameObject[] pitRooms;
    public Transform pitRoomPosition;
    public bool isDeadly;

    public void FallInPit(GameObject player) 
    {
        StopAllCoroutines();
        StartCoroutine(Fall(player)); 
    }

    IEnumerator Fall(GameObject player) 
    {
        var movement = player.GetComponent<MovementBob>();
        movement.enabled = false;
        var anim = player.GetComponent<AnimationMovementController>();

        anim.Play("Start Falling");
        yield return anim.WaitForAnimationToStop("Start Falling");

        var temp = mainCamera.cullingMask;
        canvas.SetActive(false);
        mainCamera.cullingMask = playerMask;
        yield return anim.WaitForAnimationToStop("Falling");

        if (isDeadly)
        {
            Application.Quit();
            yield break;
        }

        var pitRoom = Instantiate(pitRooms[Random.Range(0, pitRooms.Length)], pitRoomPosition.position, Quaternion.identity);

        anim.Play("Idle");
        mainCamera.cullingMask = temp;
        canvas.SetActive(true);
        player.transform.position = pitRoom.transform.position;
        movement.enabled = true;
    }
}
