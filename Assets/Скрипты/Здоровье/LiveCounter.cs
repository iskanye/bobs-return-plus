using UnityEngine;
using UnityEngine.UI;

public class LiveCounter : MonoBehaviour
{
    public Image[] lives;
    public new CameraShake camera;
    public int maxLives = 3;
    public int livesRemaining;

    public static LiveCounter Self { get; private set; }

    bool isInvincible = false;
    float invincibleTime = float.PositiveInfinity;

    void Awake() => LiveCounter.Self = this;

    void Update()
    {
        // ������� ��� �����
        if (Input.GetKeyDown(KeyCode.K)) 
            LoseLife();

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
        var self = Self;

        if (self.isInvincible)
            return;

        self.livesRemaining--;

        self.camera.StartShake();

        self.lives[self.livesRemaining].gameObject.SetActive(false); 
        self.isInvincible = true;  

        if (self.livesRemaining == 0) Application.Quit();
    }

    public static bool Heal()
    {
        var self = Self;

        if (self.livesRemaining == self.maxLives) 
            return false;

        self.lives[self.livesRemaining].gameObject.SetActive(true); 
        self.livesRemaining++;

        return true;
    }
}
