using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Active { get; private set; }

    PlayerInput input;

    void Awake()
    {
        input = GetComponent<PlayerInput>();
        Active = this;
    }

    public void AddListenerToActionStarted(string actionName, System.Action<InputAction.CallbackContext> listener) =>
        input.currentActionMap[actionName].started += listener;

    public void AddListenerToActionPerformed(string actionName, System.Action<InputAction.CallbackContext> listener) =>
        input.currentActionMap[actionName].performed += listener;

    public void AddListenerToActionCanceled(string actionName, System.Action<InputAction.CallbackContext> listener) =>
        input.currentActionMap[actionName].canceled += listener;
}
