using UnityEngine;

public class ChangeActionBaseEvent : MonoBehaviour 
{
    public ActionBase actionBase;
    public UnityEngine.Events.UnityEvent<GameObject> action;

    public void Change() => actionBase.action = action;
}