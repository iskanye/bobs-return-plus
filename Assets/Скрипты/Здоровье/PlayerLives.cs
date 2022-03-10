public class PlayerLives : UnityEngine.MonoBehaviour, ILives
{
    public int Lives
    {
        get => 
            PlayerLiveCounter.Active.LivesRemaining;

        set => 
            PlayerLiveCounter.Active.LivesRemaining = value;
    }
}
