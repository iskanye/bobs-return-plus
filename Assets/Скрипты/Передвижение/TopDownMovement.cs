using UnityEngine;
using UnityEngine.InputSystem;

//Тестовый скрипт передвижения
public class TopDownMovement : BaseMovement
{
    public override bool IsWalking => dir != Vector2.zero;

    [HideInInspector] public Vector2 dir; //Направление

    Rigidbody2D rig; //Физика обьекта

    //Находим компонент физики
    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        Direction = Vector2.down;
    }

    void OnEnable() 
    {
        InputManager.Input.Player.Move.performed += Input;
        InputManager.Input.Player.Move.canceled += InputStop;
    }

    void OnDisable()
    {
        InputManager.Input.Player.Move.performed -= Input;
        InputManager.Input.Player.Move.canceled -= InputStop;
        rig.velocity = dir = Vector2.zero;
    }

    void Update()
    {
        if (IsWalking) 
            Direction = dir;
    }

    void FixedUpdate()
    {
        rig.velocity = dir.normalized * speed; //Переводим кадры в секунды и прикладываем к обьекту скорость
    }

    public void Input(InputAction.CallbackContext c) => 
        dir = c.ReadValue<Vector2>();

    public void InputStop(InputAction.CallbackContext c) =>
        dir = Vector2.zero; 
}
