[UnityEngine.CreateAssetMenu(fileName = "Healing Item", menuName = "Items/Healing Item", order = 0)]
public class HealingItem : Item
{
    public int healing = 1;
    public EffectBase effect;

    public override bool Action()
    {
        if (PlayerLiveCounter.Active.LivesRemaining == PlayerLiveCounter.Active.maxLives || PlayerLiveCounter.Active.isInvincible)
            return false;

        PlayerLiveCounter.Active.LivesRemaining += healing;

        if (effect != null)
            PlayerEffectsController.Active.AddEffect(effect);

        return true;
    }
}