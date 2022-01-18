using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public LayerMask playerMask; 
    public UnityEvent actionOnEnter; 
    public UnityEvent actionOnStay; 

    void OnTriggerEnter2D(Collider2D c) 
    {
        if (1 << c.gameObject.layer == playerMask.value) actionOnEnter.Invoke(); 
    }

    void OnTriggerStay2D(Collider2D c) 
    {
        if (1 << c.gameObject.layer == playerMask.value) actionOnStay.Invoke(); 
    }
}
