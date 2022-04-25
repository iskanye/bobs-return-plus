public class PlayerLives : LivesBase
{
    public override int Lives
    {
        get =>
            PlayerLiveCounter.Active.LivesRemaining;

        set
        {
            if (livesCalculation == null)
                PlayerLiveCounter.Active.LivesRemaining = value;

            else
                PlayerLiveCounter.Active.LivesRemaining = livesCalculation.Invoke(value);
        }
    }

    public int[] livesInOneHeart;

    void Awake()
    {
        PlayerLiveCounter.Active.livesInOneHeart = livesInOneHeart;
        PlayerLiveCounter.Active.Initialize();
    }
}
