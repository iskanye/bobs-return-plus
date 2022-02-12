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
    
    public static void LoseLife()
    {
        var self = Active;

        if (self.isInvincible)
            return;

        self.livesRemaining--;

        CameraController.StartShake();

        self.lives[self.livesRemaining].gameObject.SetActive(false); 
        self.isInvincible = true;  

        if (self.livesRemaining == 0) Application.Quit();
    }

    public static bool Heal()
    {
        var self = Active;

        if (self.livesRemaining >= self.maxLives) 
            return false;

        self.lives[self.livesRemaining].gameObject.SetActive(true); 
        self.livesRemaining++;

        return true;
    }
}
