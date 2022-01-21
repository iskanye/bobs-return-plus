using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public LayerMask playerMask; 
    public bool isOnStay;
    public UnityEvent action; 

    void OnTriggerEnter2D(Collider2D c) 
    {
        if (1 << c.gameObject.layer == playerMask.value && !isOnStay) action.Invoke(); 
    }

    void OnTriggerStay2D(Collider2D c) 
    {
        if (1 << c.gameObject.layer == playerMask.value && isOnStay) action.Invoke(); 
    }
}
