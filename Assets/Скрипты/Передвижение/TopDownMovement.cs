using UnityEngine;

//Тестовый скрипт передвижения
public class TopDownMovement : BaseMovement
{
    public override bool IsWalking => dir != Vector2.zero && isEnabled;
    private bool isEnabled = true;

    Vector2 dir => InputManager.Active.direction; //Направление

    Rigidbody2D rig; //Физика обьекта

    //Находим компонент физики
    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        Direction = Vector2.down;
    }

    void Update()
    {
        if (IsWalking) 
            Direction = dir;
    }

    void FixedUpdate()
    {
        if (!isEnabled)
            return;

        rig.velocity = dir.normalized * speed;
    }

    //Добавил ссылку на объект который вызывает эти функции в параметры
    //Чтобы можно было всегда продебажить и понять кто вкл/откл движение игрока
    public void Enable() 
    {
        isEnabled = true;
    }
    
    public void Disable()
    {
        isEnabled = false;
        rig.velocity = Vector2.zero;
    }
}
