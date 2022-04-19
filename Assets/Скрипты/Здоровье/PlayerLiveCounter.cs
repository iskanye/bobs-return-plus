using UnityEngine;
using System.Collections;

public class PlayerLiveCounter : MonoBehaviour
{
    public Animator[] lives;
    public int[] livesInOneHeart;
    public float invincibleTime = 2;

    public int maxLives 
    {
        get
        {
            int res = 0;

            for (int i = 0; i < livesInOneHeart.Length; i++)
                res += livesInOneHeart[i];

            return res;
        } 
    }

    public int LivesRemaining 
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

            if (value == livesRemaining)
                return;

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
    int livesRemaining;
    int heart;
    int livesInHeart;

    void Awake() =>
        Active = this;

    public void Initialize()
    {
        for (int i = livesInOneHeart.Length; i < lives.Length; i++)
            lives[i - 1].GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, 0);

        StartCoroutine(ChangeLives(SceneData.Data.lives != null ? (int)SceneData.Data.lives : maxLives));
    }

    IEnumerator ChangeLives(int val)
    {
        var cache = livesRemaining;

        do
        {
            if (val >= livesRemaining)
            {
                if (livesInHeart > livesInOneHeart[heart])
                {
                    heart++;
                    livesInHeart = 1;
                }

                livesInHeart++;
                livesRemaining++;

                lives[heart].SetTrigger("Heal");
            }

            else
            {
                lives[heart].SetTrigger("Hurt");
                livesInHeart--;
                livesRemaining--;

                if (livesInHeart < 1)
                {
                    heart--;
                    livesInHeart = livesInOneHeart[heart];
                }
            }

            yield return new WaitForSeconds(.4f);
        } while (val != livesRemaining);

        if (val > cache)
            lives[heart].SetTrigger("Heal");

        for (int i = heart; i < livesInOneHeart.Length - 1; i++)
            if (livesInOneHeart[i] == 3)
                livesInOneHeart[i] = 2;
    }

    void Update()
    {
        for (int i = 0; i < livesInOneHeart.Length; i++)
            lives[i].SetInteger("Lives", livesInOneHeart[i]);

        if (float.IsPositiveInfinity(invincibleDelay) && isInvincible)
            invincibleDelay = Time.time + invincibleTime;

        if (Time.time >= invincibleDelay) 
        {
            invincibleDelay = float.PositiveInfinity;
            isInvincible = false;
        }
    }
}
