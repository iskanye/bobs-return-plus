using UnityEngine;

[CreateAssetMenu(fileName = "Fortune Bread", menuName = "Items/Special/Fortune Bread", order = 0)]
public class FortuneBread : HealingItem
{
    public UninteractiveDialogue dontHaveAnything;
    public UninteractiveDialogue haveAxe;

    public override bool Action()
    {
        if (PlayerLiveCounter.Active.LivesRemaining == PlayerLiveCounter.Active.maxLives || PlayerLiveCounter.Active.isInvincible)
            return false;

        PlayerLiveCounter.Active.LivesRemaining += healing;

        var prevDialogue = mn.activator.dialogues;

        if (PlayerPrefs.HasKey("travel_axe"))
            mn.activator.dialogues = haveAxe;

        else
            mn.activator.dialogues = dontHaveAnything;

        mn.activator.Dialogue();
        mn.activator.dialogues = prevDialogue;

        return true;
    }    
}
