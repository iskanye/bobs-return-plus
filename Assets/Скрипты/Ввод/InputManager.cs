using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputMap Input { get; private set; }
    static InputManager Active;

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
    }
}
