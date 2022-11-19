using UnityEngine;
using UnityEngine.Events;

public abstract class ActionBase : MonoBehaviour 
{
    public UnityEvent<GameObject> action = new UnityEvent<GameObject>();
}