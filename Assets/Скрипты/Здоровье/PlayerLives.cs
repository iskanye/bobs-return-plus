public class PlayerLives : LivesBase
{
    public override int Lives
    {
        get => 
            PlayerLiveCounter.Active.LivesRemaining;

        set => 
            PlayerLiveCounter.Active.LivesRemaining = value;
    }

    public int[] livesInOneHeart;

    void Awake()
    {
        PlayerLiveCounter.Active.livesInOneHeart = livesInOneHeart;
        PlayerLiveCounter.Active.Initialize();
    }
}
