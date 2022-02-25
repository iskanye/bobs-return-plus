using UnityEngine;

public class Trigger : ActionBase
{
    public LayerMask playerMask; 
    public bool isOnStay;

    void OnTriggerEnter2D(Collider2D c) 
    {
        if (action != null && ((1 << c.gameObject.layer) | playerMask) == playerMask && !isOnStay) action.Invoke(c.gameObject); 
    }

    void OnTriggerStay2D(Collider2D c) 
    {
        if (action != null && ((1 << c.gameObject.layer) | playerMask) == playerMask && isOnStay) action.Invoke(c.gameObject); 
    }
}
