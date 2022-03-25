public class PlayerLives : UnityEngine.MonoBehaviour, ILives
{
    public int Lives
    {
        get => 
            (int)PlayerLiveCounter.Active.LivesRemaining;

        set => 
            PlayerLiveCounter.Active.LivesRemaining = value;
    }
}
