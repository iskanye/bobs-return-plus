using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Active { get; private set; }

    [HideInInspector] public PlayerInput input;

    void Awake()
    {
        input = GetComponent<PlayerInput>();
        Active = this;
    }

    public static void AddListenerToActionStarted(string actionName, System.Action<InputAction.CallbackContext> listener) =>
        Active.input.currentActionMap[actionName].started += listener;

    public static void AddListenerToActionPerformed(string actionName, System.Action<InputAction.CallbackContext> listener) =>
        Active.input.currentActionMap[actionName].performed += listener;

    public static void AddListenerToActionCanceled(string actionName, System.Action<InputAction.CallbackContext> listener) =>
       Active.input.currentActionMap[actionName].canceled += listener;
}
