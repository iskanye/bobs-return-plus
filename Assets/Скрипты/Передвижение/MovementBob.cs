using UnityEngine;
using UnityEngine.InputSystem;

//Тестовый скрипт передвижения
public class MovementBob : MonoBehaviour, IWalkable
{
    public float speed; //Скорость передвижения(за кадр)
    
    Rigidbody2D rig; //Физика обьекта
    Vector2 dir; //Направление

    public bool IsWalking => dir != Vector2.zero; 

    public Vector2 Direction { get; private set; }

    //Находим компонент физики
    void Awake() =>
        rig = GetComponent<Rigidbody2D>();

    void Start()
    {
        InputManager.Active.AddListenerToActionPerformed("Move", Input);
        InputManager.Active.AddListenerToActionCanceled("Move", e => InputStop());
    }

    void Update()
    {
        //Считываем ввод клавиатуры и изменяем направление
        //dir = new Vector2(
            //Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0,
            //Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0);
        //ЗАМЕТКА: тут я использую тернарную операцию для сокращения и упрощения кода: условие ? выражение если верно : выражение иначе
        //Отправляем данные в контроллер анимаций
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
