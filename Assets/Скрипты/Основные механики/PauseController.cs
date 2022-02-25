using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject pauseBox;

    bool isPaused;

    void Awake() =>
        InputManager.AddListenerToActionStarted("Pause", Input);

    void Update()
    {
        pauseBox.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void Input(UnityEngine.InputSystem.InputAction.CallbackContext c) => 
        isPaused = !isPaused;

    public void Continue() => 
        isPaused = false;

    public void Quit() =>
        Application.Quit();
}
