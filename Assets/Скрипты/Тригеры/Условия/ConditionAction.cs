using UnityEngine;
using System.Linq;

public class ConditionAction : ActionBase
{
    public enum ConditionsCheck { All, Any }
    public ConditionsCheck conditionsCheck;
    public ConditionBase[] conditions;

    GameObject obj;

    public void Action(UnityEngine.GameObject obj)
    {
        if ((conditionsCheck == ConditionsCheck.All && conditions.All(i => i.Condition())) ||
            (conditionsCheck == ConditionsCheck.Any && conditions.Any(i => i.Condition())))
            action.Invoke(obj);
    }
}