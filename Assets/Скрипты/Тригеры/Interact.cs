using UnityEngine;

public class Interact : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent action;

    void OnTriggerStay2D(Collider2D c) 
    {
        if (c.CompareTag("Player") && Input.GetKeyDown(KeyCode.Space)) action.Invoke(); 
    }
}
