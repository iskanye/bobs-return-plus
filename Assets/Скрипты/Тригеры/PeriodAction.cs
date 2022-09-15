using UnityEngine;

public class PeriodAction : ActionBase
{
    public float startDelay;
    public float period;

    GameObject obj;

    public void StartAction()
    {
        CancelInvoke();
        InvokeRepeating("Do", startDelay, period);
    }

    public void StartAction(GameObject obj)
    {
        this.obj = obj;
        StartAction();
    }

    void Do() 
    {
        if (action != null)
            if (obj == null)
                action.Invoke(gameObject);

            else
                action.Invoke(obj);
    }

    public void StopAction() =>
        CancelInvoke();
}
