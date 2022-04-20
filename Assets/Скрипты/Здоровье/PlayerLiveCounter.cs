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
                CameraController.StartShake();

            StartCoroutine(ChangeLives(value));
        }
    }

    public static PlayerLiveCounter Active { get; private set; }

    [HideInInspector] public bool isInvincible;

    int livesRemaining;
    int heart;
    int livesInHeart;

    void Awake() =>
        Active = this;

    public void Initialize()
    {
        for (int i = livesInOneHeart.Length; i < lives.Length; i++)
            lives[i - 1].GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, 0);

        StartCoroutine(ChangeLives(SceneData.Data.lives != -1 ? SceneData.Data.lives : maxLives));
    }

    IEnumerator ChangeLives(int val)
    {
        isInvincible = true;
        var cache = livesRemaining;

        while (val != livesRemaining)
        {
            if (val >= livesRemaining)
            {
                livesInHeart++;

                if (livesInHeart > livesInOneHeart[heart])
                {
                    heart++;
                    livesInHeart = 1;
                }

                livesRemaining++;
                lives[heart].SetTrigger("Heal");
            }

            else
            {
                livesInHeart--;

                if (livesInHeart < 0)
                {
                    heart--;
                    livesInHeart = livesInOneHeart[heart] - 1;
                }

                livesRemaining--;
                lives[heart].SetTrigger("Hurt");
            }

            yield return new WaitForSeconds(.3f);
        } 

        if (val < cache)
            yield return new WaitForSeconds(invincibleTime);

        for (int i = heart + 1; i < livesInOneHeart.Length; i++)
            if (livesInOneHeart[i] == 3)
                livesInOneHeart[i] = 2;

        isInvincible = false;
    }

    void Update()
    {
        for (int i = 0; i < livesInOneHeart.Length; i++)
            lives[i].SetInteger("Lives", livesInOneHeart[i]);
    }
}
