using UnityEngine;
using UnityEngine.UI;

public class LiveCounter : MonoBehaviour
{
    public Image[] lives;
    public int maxLives = 3;
    public int livesRemaining;

    public static LiveCounter Active { get; private set; }

    bool isInvincible;
    float invincibleTime = float.PositiveInfinity;

    void Awake() => Active = this;

    void Start() 
    {
        var data = SceneData.Data;
        livesRemaining = data.lives;

        for (int i = 0; i < lives.Length; i++) lives[i].gameObject.SetActive(i <= data.lives - 1);
    }

    void Update()
    {
        if (float.IsPositiveInfinity(invincibleTime) && isInvincible) 
            invincibleTime = Time.time + 2;

        if (Time.time >= invincibleTime) 
        {
            invincibleTime = float.PositiveInfinity;
            isInvincible = false;
        }
    }

    public void LoseLifes(int lifes)
    {
        if (isInvincible || livesRemaining == 0)
            return;

        for (int i = 0; i < lifes; i++)
        {
            livesRemaining--;
            lives[livesRemaining].gameObject.SetActive(false);

            if (livesRemaining == 0)
            {
                Application.Quit();
                return;
            }
        }

        CameraController.StartShake();
        isInvincible = true;
    }

    public bool Heal()
    {
        if (livesRemaining >= maxLives) 
            return false;

        lives[livesRemaining].gameObject.SetActive(true); 
        livesRemaining++;

        return true;
    }
}
