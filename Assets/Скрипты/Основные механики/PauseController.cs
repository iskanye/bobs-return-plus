using UnityEngine;

public class PauseController : MonoBehaviour, IInputListener
{
    public GameObject pauseBox;

    bool isPaused;

    void Start() =>
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

    public void DeleteListeners() =>
        InputManager.RemoveListenerAtActionStarted("Pause", Input);
}
