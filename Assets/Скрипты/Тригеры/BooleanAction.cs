public class BooleanAction : ActionBase
{
    public void Action(bool b) 
    {
        if (b && action != null)
            action.Invoke(gameObject);
    }
}
