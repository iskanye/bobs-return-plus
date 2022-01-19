using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LiveCounter : MonoBehaviour
{
    public Image[] lives;
    public int livesRemaining;
    
    public void LoseLife()
    {
        livesRemaining--;

        lives[livesRemaining].gameObject.SetActive(false);

        if(livesRemaining==0)
        {
            Debug.Log("проебал");
        }    
    }

    private void Update()
    {
        // сделано для теста
        if (Input.GetKeyDown(KeyCode.K))
        {
            LoseLife();
        }
    }
}
