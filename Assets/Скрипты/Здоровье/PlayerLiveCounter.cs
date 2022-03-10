using UnityEngine;

public class PlayerLiveCounter : MonoBehaviour
{
    public UnityEngine.UI.Image[] lives;
    public int maxLives = 3;
    public float invincibleTime = 2;

    public int LivesRemaining 
    {
        get => 
            livesRemaining;

        set 
        {
            if (isInvincible || livesRemaining == 0)
                return;

            if (value - livesRemaining < 0) 
            {
                CameraController.StartShake();
                isInvincible = true;
            }

            if (value <= maxLives)
                livesRemaining = value;

            else
                livesRemaining = maxLives;

            for (int i = 0; i < lives.Length; i++)
                lives[i].gameObject.SetActive(i <= value - 1);

            if (livesRemaining == 0)
            {
                Application.Quit();
                return;
            }
        }
    }

    public static PlayerLiveCounter Active { get; private set; }

    bool isInvincible;
    float invincibleDelay = float.PositiveInfinity;
    int livesRemaining;

    void Awake() => Active = this;

    void Start() 
    {
        var data = SceneData.Data;
        livesRemaining = data.lives;

        for (int i = 0; i < lives.Length; i++) 
            lives[i].gameObject.SetActive(i <= data.lives - 1);
    }

    void Update()
    {
        if (float.IsPositiveInfinity(invincibleDelay) && isInvincible)
            invincibleDelay = Time.time + invincibleTime;

        if (Time.time >= invincibleDelay) 
        {
            invincibleDelay = float.PositiveInfinity;
            isInvincible = false;
        }
    }

    //public void LoseLifes(int lifes)
    //{
    //    if (isInvincible || LivesRemaining == 0)
    //        return;

    //    for (int i = 0; i < lifes; i++)
    //    {
    //        LivesRemaining--;
    //        lives[LivesRemaining].gameObject.SetActive(false);

    //        if (LivesRemaining == 0)
    //        {
    //            Application.Quit();
    //            return;
    //        }
    //    }

    //    CameraController.StartShake();
    //    isInvincible = true;
    //}

    //public bool Heal(int lifes)
    //{
    //    if (LivesRemaining >= maxLives) 
    //        return false;

    //    LivesRemaining += lifes;

    //    return true;
    //}
}
