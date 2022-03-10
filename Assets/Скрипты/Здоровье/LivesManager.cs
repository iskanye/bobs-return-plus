using UnityEngine.Events;

public class LivesManager : UnityEngine.MonoBehaviour, ILives
{
    public int Lives
    {
        get => 
            lives;

        set
        {
            if (value - lives < 0 && value != 0 && onHit != null)
                onHit.Invoke();

            if (value == 0 && onDeath != null)
                onDeath.Invoke();

            lives = value;
        }
    }

    public UnityEvent onDeath;
    public UnityEvent onHit;

    int lives;
}
