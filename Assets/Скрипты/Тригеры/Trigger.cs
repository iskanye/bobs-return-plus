using UnityEngine;

public class Trigger : ActionBase
{
    public LayerMask playerMask; 
    public bool isOnStay;

    void OnTriggerEnter2D(Collider2D c) 
    {
        if (1 << c.gameObject.layer == playerMask.value && !isOnStay) action.Invoke(); 
    }

    void OnTriggerStay2D(Collider2D c) 
    {
        if (1 << c.gameObject.layer == playerMask.value && isOnStay) action.Invoke(); 
    }
}
