public class NoHitChallenge : ChallengeBase
{
    public PlayerWarp warp;

    bool hit;
    PlayerLives lives;

    public override void StartChallenge()
    {
        lives = warp.player.data.lives;
        lives.livesCalculation += NoHit;
    }

    public override void StopChallenge()
    {
        lives.livesCalculation -= NoHit;

        if (!hit)
            ifChallengeDone.Invoke();
    }

    int NoHit(int newLives) 
    {
        if (PlayerLiveCounter.Active.LivesRemaining > newLives)
            hit = true;

        return newLives;
    }
}
