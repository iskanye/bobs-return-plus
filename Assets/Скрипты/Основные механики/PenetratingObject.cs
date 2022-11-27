public class PenetratingObject : ActionBase
{
    public ObjectType type;

    public ObjectType Durability { set => type = value; }

    public void Penetrate() =>
        action.Invoke(gameObject);
}
