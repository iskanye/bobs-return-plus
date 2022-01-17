using UnityEngine;
using Pathfinding;

//Скрипт ИИ
public class AI : MonoBehaviour
{
    public LayerMask obstacles, player; //Слой, видимый для ИИ
    public float seeRange; //Дальность зрения
    public float spotSpeed; //Скорость движения(При погоне)
    public Vector2 direction = Vector2.right;
    [Range(0, 360)] public float viewAngle;
    [Header("Patrol AI")] public bool isPatrol; //Является ли данный ИИ патрульным     
    public float patrolSpeed; //Скорость движения(При патрулировании)
    public Vector2[] path; //Массив пути(для патрульного ИИ)
    [HideInInspector] public bool isSee; //Видит ли ИИ игрока?
    [HideInInspector] public Vector2 target; //Позиция цели

    AnimationMovementController anim; //Контроллер анимаций
    AILerp ai; //Скрипт поиска пути
    bool patr; //Патрулирует ли ИИ?
    int currWay = 0; //Текущий путь из массива позиций
    float swTime; //Время ожидания(для патруля)

    void Awake()
    {
        //Получаем скрипт поиска пути и контроллер анимаций
        ai = GetComponent<AILerp>();
        anim = GetComponent<AnimationMovementController>();
        patr = isPatrol;
    }

    void Update()
    {
        var check = Physics2D.OverlapCircle(transform.position, seeRange, player); //Проверяем наличие игрока в радиусе поиска

        if (check != null)
        {
            //Проверяем угол между игроком и ИИ
            if (Mathf.Acos(Vector2.Dot(direction, (check.transform.position - transform.position).normalized)) * Mathf.Rad2Deg < viewAngle * .5f) 
            {
                //Если угол укладывается в поле зрения, то мы проверяем на наличие препятствий между ИИ и игроком
                if (!Physics2D.Raycast(transform.position, (check.transform.position - transform.position).normalized,
                (check.transform.position - transform.position).magnitude, obstacles))
                {
                    //Препятствий нет - начинаем преследовать игрока             
                    target = check.transform.position;
                    direction = (check.transform.position - transform.position).normalized;
                    isSee = true;
                }
                //Препятствие есть - ИИ не видит игрока
                else isSee = false;
            }
            //Угол не укладывается - ИИ не видит игрока
            else isSee = false;
        }

        //Если каким-то образом ИИ думает что видит игрока, но его нет в зоне досигаемости, то ИИ больше игрока не видит
        else if (isSee) isSee = false;

        //Если ИИ уже как [2;4] секунды не видел игрока, то(Если ИИ патрульный) начинаем патрулировать нашу зону патруля
        if (ai.reachedEndOfPath && !isSee && !patr && isPatrol) Invoke("StartPatroling", Random.Range(2, 4));

        //Если ИИ видит игрока
        if (isSee)
        {
            patr = false; //Отключаем патруль
            currWay = 0;             
            swTime = float.PositiveInfinity;
            ai.speed = spotSpeed; //Меняем ему скорость
            ai.destination = target; //Меняем ему цель на игрока
            ai.SearchPath(); //Ищем путь до игрока
        }

        //Если ИИ не видит игрока, но он патрульный
        else if (patr)
        {
            ai.speed = patrolSpeed; //Меняем скорость на обычную
            var search = false; //Надо ли нам искать путь?

            //Если ИИ достиг конца пути, не ищет путь и его время ожидания равно бесконечности, 
            //то мы назначаем ему время после которого ему надо будет идти к другой точке патруля
            if (ai.reachedEndOfPath && !ai.pathPending && float.IsPositiveInfinity(swTime)) swTime = Time.time + Random.Range(.5f, 6);

            //Если пора искать путь
            if (Time.time >= swTime)
            {
                currWay++; //Обновляем путь
                search = true; //Да, нам надо искать путь
                swTime = float.PositiveInfinity; //Делаем время ожидания недосигаемым
            }

            currWay %= path.Length; //Вычисляем остаток от деления текущего пути на длины массива путей, чтобы текущий путь не превышал кол-во путей
            ai.destination = path[currWay]; //Назначаем ИИ путь

            if (search) ai.SearchPath(); //Если надо ищем этот самый путь

        }

        //Если ИИ ничего не видит, и направление его пути не равно нулю, то направление его зрения должно быть равно направлению его движения
        if (!isSee && ai.velocity != Vector3.zero) direction = ai.velocity.normalized;

        //Отправляем контроллеру анимаций данные передвижениях ИИ
        anim.isWalk = !ai.reachedEndOfPath;
        anim.direction = direction;
    }

    //Система реагирования на скриптовые шумы
    public void NoiseDistraction(Vector2 noise)
    {
        patr = false; //Отключаем патруль      
        ai.speed = spotSpeed; //Меняем скорость
        ai.destination = noise; //Назначем цель на место шума   
        ai.SearchPath(); //Ищем путь до места шума
    }

    //Функция начала путрлирования, после того как ИИ некоторое время не видел игрока
    void StartPatroling() => patr = true;

    //Для наглядности рисуем точки патруля желтыми кружками
    void OnDrawGizmos() 
    {
        if (isPatrol)
        {
            Gizmos.color = Color.yellow;

            foreach (var i in path) Gizmos.DrawWireSphere(i, .2f);
        }
    }
}
