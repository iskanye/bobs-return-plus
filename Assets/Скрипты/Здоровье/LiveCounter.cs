using UnityEngine;
using UnityEngine.UI;

public class LiveCounter : MonoBehaviour
{
    public Image[] lives;
    public new CameraShake camera;
    public int livesRemaining;

    bool isInvincible = false;
    float invincibleTime = float.PositiveInfinity;

    private void Update()
    {
        // ������� ��� �����
        if (Input.GetKeyDown(KeyCode.K)) LoseLife();

        if (float.IsPositiveInfinity(invincibleTime) && isInvincible) invincibleTime = Time.time + 2;

        if (Time.time >= invincibleTime) 
        {
            invincibleTime = float.PositiveInfinity;
            isInvincible = false;
        }
    }
    
    public void LoseLife()
    {
        if (isInvincible)
        {
            Debug.Log("�������");
            return;
        } 

        livesRemaining--;

        camera.StartShake();

        lives[livesRemaining].gameObject.SetActive(false); 
        isInvincible = true;  

        if (livesRemaining <= 0) Application.Quit();
    }
}
