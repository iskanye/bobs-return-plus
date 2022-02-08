using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject pauseBox;

    bool isPaused;

    void Update()
    {
        pauseBox.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void Input() => 
        isPaused = !isPaused;

    public void Continue() => 
        isPaused = false;

    public void Quit() =>
        Application.Quit();
}
