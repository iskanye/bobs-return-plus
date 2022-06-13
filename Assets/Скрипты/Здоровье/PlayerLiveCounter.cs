using UnityEngine;

public class PlayerLiveCounter : MonoBehaviour
{
    public Animator[] lives;
    public Heart[] livesInOneHeart;
    public float invincibleTime = 2;

    public int maxLives 
    {
        get
        {
            int res = 0;

            for (int i = 0; i < livesInOneHeart.Length; i++)
                res += livesInOneHeart[i].lives;

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

    System.Collections.IEnumerator ChangeLives(int val)
    {
        isInvincible = true;
        var cache = livesRemaining;

        while (val != livesRemaining)
        {
            if (val >= livesRemaining)
            {
                livesInHeart++;

                if (livesInHeart > livesInOneHeart[heart].lives)
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
                    livesInOneHeart[heart].type = LiveType.Regular;

                    if (livesInOneHeart[heart].type == LiveType.Backpack || livesInOneHeart[heart].type == LiveType.Shield)
                        livesInOneHeart[heart].lives = 2;

                    heart--;
                    livesInHeart = livesInOneHeart[heart].lives - 1;
                }

                livesRemaining--;
                lives[heart].SetTrigger("Hurt");
            }

            yield return new WaitForSeconds(.3f);
        } 

        if (val < cache)
            yield return new WaitForSeconds(invincibleTime);

        for (int i = heart + 1; i < livesInOneHeart.Length; i++)
            if (livesInOneHeart[i].lives == 3)
                livesInOneHeart[i].lives = 2;

        isInvincible = false;
    }

    void Update()
    {
        for (int i = 0; i < livesInOneHeart.Length; i++)
            lives[i].SetInteger("Lives", livesInOneHeart[i].lives);
    }
}

public enum LiveType 
{
    Regular,
    Shield,
    Backpack,
    Poisonous,
    Cold,
    Radioactive
}
[System.Serializable]
public struct Heart 
{
    public int lives;
    public LiveType type; 
}