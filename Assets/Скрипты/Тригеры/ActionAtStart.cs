public class ActionAtStart : ActionBase
{
    void Start() =>
        action.Invoke(FindObjectOfType<MovementBob>().gameObject);
}
