using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject pauseBox;
    public UnityEngine.UI.Toggle fullscreenToggle;

    bool isPaused;

    void Awake()
    {
        InputManager.Input.Player.Pause.started += Input;
        fullscreenToggle.isOn = Screen.fullScreen;
    }

    void Update()
    {
        pauseBox.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void Input(UnityEngine.InputSystem.InputAction.CallbackContext c) =>
        Continue();

    public void Continue() =>
        isPaused = !isPaused;

    public void Quit() =>
        Application.Quit();

    public void FullScreen(bool p)
    {
        Screen.fullScreen = p;
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }

    public void Pause() =>
        isPaused = true;
}
