public class ActionAtStart : ActionBase
{
    public bool isAwake;

    void Awake()
    {
        if (action != null && isAwake)
            action.Invoke(gameObject);
    }

    void Start()
    {
        if (action != null && !isAwake)
            action.Invoke(gameObject);
    }
}