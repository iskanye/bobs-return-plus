using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputMap Input;

    public static InputManager Active;

    [HideInInspector] public bool submit;
    [HideInInspector] public Vector2 direction;

    void Awake()
    {
        if (Active == null)
        {
            Active = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
            return;
        }

        Input = new InputMap();
        Input.Player.Enable();

        Input.Player.Submit.started += (e) => submit = true;
        Input.Player.Submit.canceled += (e) => submit = false;
#if !UNITY_ANDROID
        Input.Player.Move.performed += c => direction = c.ReadValue<Vector2>();
        Input.Player.Move.canceled += c => direction = Vector2.zero;
#endif
    }
}
