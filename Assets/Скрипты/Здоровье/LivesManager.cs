using UnityEngine.Events;

public class LivesManager : UnityEngine.MonoBehaviour, ILives
{
    public int maxLives;

    public int Lives
    {
        get => 
            lives;

        set
        {
            if (lives == 0)
                return;

            if (value == 0 && onDeath != null)
            {
                onDeath.Invoke();
                return;
            }

            if (value - lives < 0 && onHit != null)
                onHit.Invoke();

            lives = value;
        }
    }

    public UnityEvent onDeath;
    public UnityEvent onHit;

    int lives;

    void Awake() =>
        lives = maxLives;
}
