public class PeriodAction : ActionBase
{
    public float startDelay;
    public float period;

    void Start() =>
        InvokeRepeating("Do", startDelay, period);

    void Do() 
    {
        if (action != null)
            action.Invoke(FindObjectOfType<TopDownMovement>().gameObject); 
    }
}
