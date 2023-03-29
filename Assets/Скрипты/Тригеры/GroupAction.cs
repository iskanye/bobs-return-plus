public class GroupAction : ActionBase
{
    public void Action(UnityEngine.GameObject obj) =>
        action?.Invoke(obj);

    public void Action() =>
        Action(null);
}
