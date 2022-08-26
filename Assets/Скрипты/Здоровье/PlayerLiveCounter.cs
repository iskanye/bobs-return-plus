using UnityEngine;

public class PlayerLiveCounter : MonoBehaviour
{
    public Animator[] lives;
    public Heart[] livesInOneHeart;

    public int maxLives 
    {
        get
        {
            int res = 0;

            foreach (var i in livesInOneHeart)
                res += i.lives;

            return res;
        } 
    }

    public int LivesRemaining 
    {
        get => 
            livesRemaining;

        set 
        {
            if (value <= 0)
                value = 0;

            if (value > maxLives)
                value = maxLives;

            if (value == livesRemaining)
                return;

            ChangeLives(value);
        }
    }

    public static PlayerLiveCounter Active { get; private set; }

    public bool isInvincible;

    int livesRemaining;
    int heart;
    int livesInHeart;

    void Awake() =>
        Active = this;

    public void Initialize()
    {
        for (int i = livesInOneHeart.Length; i < lives.Length; i++)
            lives[i - 1].GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, 0);

        ChangeLives(SceneData.Data.lives != -1 && SceneData.Data.version == SaveData.currentVersion ? SceneData.Data.lives : maxLives);
    }

    void ChangeLives(int val)
    {
        while (val != livesRemaining)
            if (val < livesRemaining)
            {
                livesRemaining--;
                livesInHeart--;

                if (livesInHeart < 0)
                {
                    heart--;
                    livesInHeart = livesInOneHeart[heart].lives - 1;
                }

                lives[heart].SetInteger("Lives", livesInHeart);
            }

            else
            {
                livesRemaining++;
                livesInHeart++;

                if (livesInHeart > livesInOneHeart[heart].lives)
                {
                    heart++;
                    livesInHeart = 1;
                }

                lives[heart].SetInteger("Lives", livesInHeart);
            }

        for (int i = heart + 1; i < livesInOneHeart.Length; i++)
            lives[heart].SetInteger("Lives", 0);
    }

    void Update() =>
        lives[heart].SetInteger("Lives", livesInHeart);
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
