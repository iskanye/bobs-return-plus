using UnityEngine;
using UnityEngine.InputSystem;

//Тестовый скрипт передвижения
public class MovementBob : MonoBehaviour, IWalkable
{
    public float speed; //Скорость передвижения(за кадр)

    public bool IsWalking => dir != Vector2.zero;
    public Vector2 Direction { get; private set; }

    Rigidbody2D rig; //Физика обьекта
    Vector2 dir; //Направление

    //Находим компонент физики
    void Awake() =>
        rig = GetComponent<Rigidbody2D>();

    void Start()
    {
        InputManager.AddListenerToActionPerformed("Move", Input);
        InputManager.AddListenerToActionCanceled("Move", e => InputStop());
    }

    void Update()
    {
        if (IsWalking) Direction = dir;
    }

    void FixedUpdate()
    {
        rig.velocity = dir.normalized * Time.deltaTime * speed; 
        //Переводим кадры в секунды и линейно интерполируем скорость
    }

    public void Input(InputAction.CallbackContext c) => 
        dir = c.ReadValue<Vector2>();

    public void InputStop() =>
        dir = Vector2.zero;
}
