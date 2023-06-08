using UnityEngine;

public class RatKingPointer : MonoBehaviour
{
    public Transform gameCamera;
    public Transform ratKing;
    public Transform pointer;

    void Update()
    {
        pointer.gameObject.SetActive(Mathf.Abs(ratKing.position.y - gameCamera.position.y) >= Constants.screenHeight / 64 + 2.5f && PlayerLiveCounter.Active.LivesRemaining != 0);
        pointer.position = new Vector2(ratKing.position.x - gameCamera.position.x, pointer.position.y);
    }
}
