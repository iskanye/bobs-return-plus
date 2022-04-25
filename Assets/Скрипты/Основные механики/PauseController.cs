using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject pauseBox;
    public UnityEngine.UI.Toggle fullscreenToggle;

    Vector2Int resolution;
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
        {
            resolution = new Vector2Int(Camera.current.pixelWidth, Camera.current.pixelHeight);
            Screen.SetResolution(Display.main.systemWidth, Display.main.systemWidth, true);
        }
        else
            Screen.SetResolution(resolution.x, resolution.y, false);

        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }
}
