public class PenetratingObject : ActionBase
{
    public ObjectType type;

    public ObjectType Durability(ObjectType type) => 
        this.type = type;

    public void Penetrate() =>
        action.Invoke(gameObject);
}
