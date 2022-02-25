public class ActionInTime : ActionBase
{
    public float time;

    public void Action() => 
        Invoke("Act", time);

    void Act()
    {
        if (action != null)
            action.Invoke(FindObjectOfType<MovementBob>().gameObject);
    }
}
