[UnityEngine.CreateAssetMenu(fileName = "Healing Item", menuName = "Items/Healing Item", order = 0)]
public class HealingItem : Item
{
    public bool healIfHPIsMax = false;
    public int healing = 1;
    public EffectBase[] effects;

    public override bool Action()
    {
        if ((PlayerLiveCounter.Active.LivesRemaining == PlayerLiveCounter.Active.maxLives && !healIfHPIsMax) || PlayerLiveCounter.Active.isInvincible)
            return false;

        PlayerLiveCounter.Active.LivesRemaining += healing;

        if (effects != null)
            foreach (var i in effects)
                PlayerEffectsController.Active.AddEffect(i);

        return true;
    }
}