using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager Active { get; private set; }

    [HideInInspector] public PlayerInput input;

    void Awake()
    {
        input = GetComponent<PlayerInput>();

        if (Active == null)
        {
            Active = this;
            DontDestroyOnLoad(gameObject);
        }

        else
            Destroy(gameObject);
    }

    public static void AddListenerToActionStarted(string actionName, Action<InputAction.CallbackContext> listener) =>
        Active.input.currentActionMap[actionName].started += listener;

    public static void AddListenerToActionPerformed(string actionName, Action<InputAction.CallbackContext> listener) =>
        Active.input.currentActionMap[actionName].performed += listener;

    public static void AddListenerToActionCanceled(string actionName, Action<InputAction.CallbackContext> listener) =>
        Active.input.currentActionMap[actionName].canceled += listener;

    public static void RemoveListenerAtActionStarted(string actionName, Action<InputAction.CallbackContext> listener) =>
        Active.input.currentActionMap[actionName].started -= listener;

    public static void RemoveListenerAtActionPerformed(string actionName, Action<InputAction.CallbackContext> listener) =>
        Active.input.currentActionMap[actionName].performed -= listener;

    public static void RemoveListenerAtActionCanceled(string actionName, Action<InputAction.CallbackContext> listener) =>
        Active.input.currentActionMap[actionName].canceled -= listener;
}
