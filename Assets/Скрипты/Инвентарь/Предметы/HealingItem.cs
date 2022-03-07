[UnityEngine.CreateAssetMenu(fileName = "Healing Item", menuName = "Healing Item", order = 0)]
public class HealingItem : Item
{
    public override bool Action() => LiveCounter.Active.Heal();
}