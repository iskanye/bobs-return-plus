using UnityEngine;
using UnityEngine.InputSystem;

public class RebindAction : MonoBehaviour
{
    public InputActionReference action;
    public int bindingIndex;
    public TMPro.TMP_Text bindingText;

    InputActionRebindingExtensions.RebindingOperation operation;
    InputAction _action;
    string actionBind => string.Join("", _action.GetBindingDisplayString(bindingIndex).Split(''));

    void Start()
    {
        _action = InputManager.Input.FindAction(action.name);
        bindingText.text = actionBind;
    }

    public void StartRebind()
    {
        _action.Disable();
        operation = _action.PerformInteractiveRebinding()
            .WithTargetBinding(bindingIndex)
            .WithControlsExcluding("<Mouse>")
            .WithControlsExcluding("<Keyboard>/escape")
            .OnComplete(i =>
            {
                operation.Dispose();
                bindingText.text = actionBind;
                _action.Enable();
            })
            .Start();

        bindingText.text = "Жду...";
    }

    public void ResetBinding()
    {
        _action.RemoveBindingOverride(bindingIndex);
        bindingText.text = actionBind;
    }
}
