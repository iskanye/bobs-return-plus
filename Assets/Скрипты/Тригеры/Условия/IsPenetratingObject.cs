public class IsPenetratingObject : ConditionBase
{
    public override bool Condition(UnityEngine.GameObject obj) => 
        obj.TryGetComponent<PenetratingObject>(out var i);
}