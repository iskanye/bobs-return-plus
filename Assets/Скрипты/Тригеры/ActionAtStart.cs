public class ActionAtStart : ActionBase
{
    void Start()
    {
        if (action != null)
            action.Invoke(FindObjectOfType<MovementBob>().gameObject);
    }
}
