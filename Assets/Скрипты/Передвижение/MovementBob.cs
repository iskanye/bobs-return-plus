using UnityEngine;

//Тестовый скрипт передвижения
public class MovementBob : MonoBehaviour, IWalkable
{
    public float speed; //Скорость передвижения(за кадр)
    [Range(0f, 1f)]public float accel; //Ускорение
    [Range(0f, 1f)]public float stopAccel; //Ускорение замедления
    
    Rigidbody2D rig; //Физика обьекта
    Vector2 dir; //Направление
    AnimationMovementController anim; //Контроллер анимаций

    public bool IsWalking => dir != Vector2.zero; 

    public Vector2 Direction => dir;

    void Awake()
    { 
        //Находим компонент физики и анимаций
        rig = GetComponent<Rigidbody2D>(); 
        anim = GetComponent<AnimationMovementController>();
    }

    void Update()
    {
        //Считываем ввод клавиатуры и изменяем направление
        dir = new Vector2(
            Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0,
            Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0);
        //ЗАМЕТКА: тут я использую тернарную операцию для сокращения и упрощения кода: условие ? выражение если верно : выражение иначе
        //Отправляем данные в контроллер анимаций
    }

    void FixedUpdate()
    {
        rig.velocity = Vector2.Lerp(rig.velocity, dir.normalized * Time.deltaTime * speed, 
        Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ? accel : stopAccel); 
        //Переводим кадры в секунды и линейно интерполируем скорость
    }
}
