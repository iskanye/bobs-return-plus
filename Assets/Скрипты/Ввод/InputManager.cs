using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputMap Input;

    public static InputManager Active;

    [HideInInspector] public bool submit 
    {
        get
        {
            var res = _submit;

            if (_submit)
                _submit = false;

            return res;
        }
        set =>
            _submit = value;
    }
    [HideInInspector] public bool attack 
    {
        get
        {
            var res = _attack;

            if (_attack)
                _attack = false;

            return res;
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
        Input.Player.Submit.started += (e) => _submit = true;
        Input.Player.Submit.canceled += (e) => _submit = false;

        Input.Player.Move.performed += c => direction = c.ReadValue<Vector2>();
        Input.Player.Move.canceled += c => direction = Vector2.zero;

        Input.Player.Attack.started += (e) => _attack = true;
        Input.Player.Attack.canceled += (e) => _attack = false;
#endif
    }
}
