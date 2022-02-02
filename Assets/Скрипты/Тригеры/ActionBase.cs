using UnityEngine;

public abstract class ActionBase : MonoBehaviour 
{
    public UnityEngine.Events.UnityEvent<GameObject> action;
}