using UnityEngine;

//Тестовый скрипт передвижения
public class TopDownMovement : BaseMovement
{
    public override bool IsWalking => dir != Vector2.zero && enabled;

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
        rig.velocity = dir.normalized * speed; //Переводим кадры в секунды и прикладываем к обьекту скорость
    }
}
