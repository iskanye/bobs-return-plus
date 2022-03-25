using UnityEngine;
using System.Collections;

public class PlayerLiveCounter : MonoBehaviour
{
    public Animator[] lives;
    public int maxLives = 6;
    public int livesInOneHeart = 2;
    public float invincibleTime = 2;

    public float LivesRemaining 
    {
        get => 
            livesRemaining;

        set 
        {
            if (isInvincible || livesRemaining == 0)
                return;

            if (value <= 0)
            {
                Application.Quit();
                return;
            }

            if (value > maxLives)
                value = maxLives;

            if (value < livesRemaining) 
            {
                CameraController.StartShake();
                isInvincible = true;
            }

            StartCoroutine(ChangeLives(value));
        }
    }

    public static PlayerLiveCounter Active { get; private set; }

    bool isInvincible;
    float invincibleDelay = float.PositiveInfinity;
    float livesRemaining;

    void Awake() => Active = this;

    IEnumerator ChangeLives(float val)
    {
        while (livesRemaining != val)
        {
            if (val > livesRemaining)
                livesRemaining++;

            lives[Mathf.CeilToInt(livesRemaining / livesInOneHeart) - 1].SetTrigger(val < livesRemaining ? "Hurt" : "Heal");

            if (val < livesRemaining)
                livesRemaining--;

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Start()
    {
        livesRemaining = SceneData.Data.lives;

        for (int i = 0; i < livesRemaining; i++)
        {
            lives[Mathf.CeilToInt(i / livesInOneHeart)].SetTrigger("Heal");
            yield return new WaitForEndOfFrame();
        }
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
}
