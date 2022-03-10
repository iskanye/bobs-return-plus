[UnityEngine.CreateAssetMenu(fileName = "Healing Item", menuName = "Healing Item", order = 0)]
public class HealingItem : Item
{
    public int healing = 1;

    public override bool Action()
    {
        if (PlayerLiveCounter.Active.LivesRemaining == PlayerLiveCounter.Active.maxLives)
            return false;

        PlayerLiveCounter.Active.LivesRemaining += healing;
        return true;
    }
}