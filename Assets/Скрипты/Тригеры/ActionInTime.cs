using UnityEngine;

public class ActionInTime : ActionBase
{
    public float time;

    GameObject obj;

    public void Action()
    {
        CancelInvoke();
        Invoke("Act", time);
    }

    public void Action(GameObject obj)
    {
        this.obj = obj;
        Action();
    }

    void Act()
    {
        if (action != null)
            if (obj == null)
                action.Invoke(gameObject);

            else
                action.Invoke(obj);
    }
}
