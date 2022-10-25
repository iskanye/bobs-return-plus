using UnityEngine;

public class Trigger : ActionBase
{
    public UnityEngine.Events.UnityEvent<GameObject> onExit;
    [SerializeField] private LayerMask playerMask; 
    [SerializeField] private bool isOnStay;
    [SerializeField] private bool TriggerNotExpected;
    [SerializeField] private bool ColliderNotExpected;
    
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

    bool IsTriggering(Collider2D c)
    {
        if (action != null && ((1 << c.gameObject.layer) & playerMask) != 0 && (c.isTrigger & !TriggerNotExpected || !c.isTrigger & !ColliderNotExpected))
        {
            return true;
        }

        return false;
    }
}
