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
        isPaused = !isPaused;

    public void Continue() =>
        isPaused = false;

    public void Quit() =>
        Application.Quit();

    public void FullScreen(bool p)
    {
        if (p)
            Screen.SetResolution(Display.main.systemWidth, Display.main.systemWidth, true);
        else
            Screen.fullScreen = false;

        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }
}
