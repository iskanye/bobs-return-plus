using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputMap Input;

    public static InputManager Active;

    [HideInInspector] public bool submit 
    {
        get
        {
#if !UNITY_ANDROID
            return Input.Player.Submit.WasPressedThisFrame();
#else
            var res = _submit;

            if (_submit)
                _submit = false;

            return res;
#endif
        }
        set =>
            _submit = value;
    }
    [HideInInspector] public bool attack 
    {
        get
        {
#if !UNITY_ANDROID
            return Input.Player.Attack.WasPressedThisFrame();
#else
            var res = _attack;

            if (_attack)
                _attack = false;

            return res;
#endif
        }
        set =>
            _attack = value;
    }
    [HideInInspector] public Vector2 direction;

    bool _submit;
    bool _attack;

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
#if !UNITY_ANDROID
        Input.Player.Move.performed += c => direction = c.ReadValue<Vector2>();
        Input.Player.Move.canceled += c => direction = Vector2.zero;
#endif
    }
}
