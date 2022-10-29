using UnityEngine;

public class Trigger : ActionBase
{
    public UnityEngine.Events.UnityEvent<GameObject> onExit;
    [SerializeField] private LayerMask playerMask; 
    [SerializeField] private bool isOnStay;
    [SerializeField] private bool triggerNotExpected;
    [SerializeField] private bool colliderNotExpected;
    
    void OnTriggerEnter2D(Collider2D c) 
    {
        if (IsTriggering(c) && !isOnStay) 
            action.Invoke(c.gameObject); 
    }

    void OnTriggerStay2D(Collider2D c) 
    {
        if (IsTriggering(c) && isOnStay) 
            action.Invoke(c.gameObject); 
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (IsTriggering(c))
            onExit.Invoke(c.gameObject);
    }

    bool IsTriggering(Collider2D c) => 
        action != null && ((1 << c.gameObject.layer) & playerMask) != 0 && ((c.isTrigger && !triggerNotExpected) || (!c.isTrigger && !colliderNotExpected));
}
