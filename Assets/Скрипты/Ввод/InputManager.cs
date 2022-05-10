using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputMap Input { get; private set; }

    public static InputManager Active;

    public bool submit;

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
    }
}
