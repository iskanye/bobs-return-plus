using UnityEngine.Events;

public class LivesManager : LivesBase
{
    public int maxLives;

    public override int Lives
    {
        get => 
            lives;

        set
        {
            if (lives == 0)
                return;

            if (livesCalculation != null)
                value = livesCalculation.Invoke(value);

            if (value <= 0 && onDeath != null)
            {
                onDeath.Invoke();
                lives = 0;
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
