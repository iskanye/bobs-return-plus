using UnityEngine;
using System.Collections;
using Pathfinding;

//Скрипт ИИ
public class AI : MonoBehaviour
{
    private LayerMask obstacles = 1 << 3; //Слой с препятствиями
    private LayerMask player = 1 << 6; //Слой с игроком
    //public float speed; //Скорость
    public float seeRange; //Дальность зрения
    public float spotSpeed; //Скорость движения(При погоне)
    public Vector2 direction = Vector2.right;
    [Range(0, 360)] public float viewAngle;
    [Header("Patrol AI")] public bool isPatrol; //Является ли данный ИИ патрульным     
    public float patrolSpeed; //Скорость движения(При патрулировании)
    public Vector2[] path; //Массив пути(для патрульного ИИ)
    [HideInInspector] public bool isPlayerSeen; //Видит ли ИИ игрока?
    [HideInInspector] public Transform currentTarget; //Трансформ цели

    AnimationMovementController anim; //Контроллер анимаций
    IAstarAI ai; //Скрипт поиска пути

    [SerializeField] private bool patrolling; //Патрулирует ли ИИ?
    private int currWay = 0; //Текущий путь из массива позиций
    private float currTime; //Таймер
    private float swTime; //Время ожидания(для патруля)

    void Awake()
    {
        //Получаем скрипт поиска пути и контроллер анимаций
        ai = GetComponent<AILerp>();
        anim = GetComponent<AnimationMovementController>();
        patrolling = isPatrol;
    }

    private bool CanSeePlayer()
    {
        var check = Physics2D.OverlapCircle(transform.position, seeRange, player); //Проверяем наличие игрока в радиусе поиска

        if (check == null) 
            return false;

        //Проверяем угол между игроком и ИИ
        Vector3 dirToPlayer = (check.transform.position - transform.position).normalized;
        if (Mathf.Acos(Vector2.Dot(direction, dirToPlayer)) * Mathf.Rad2Deg < viewAngle * .5f)
        {
            //Если угол укладывается в поле зрения, то мы проверяем на наличие препятствий между ИИ и игроком
            if (!Physics2D.Linecast(transform.position, check.transform.position, obstacles))
            {
                //Препятствий нет - начинаем преследовать игрока             
                currentTarget = check.transform;
                direction = dirToPlayer;
                return true;
            }
        }

        return false;
    }

    void Update()
    {
        isPlayerSeen = CanSeePlayer();

        //Если ИИ уже как [2;4] секунды не видел игрока, то(Если ИИ патрульный) начинаем патрулировать нашу зону патруля
        if (ai.reachedEndOfPath && !isPlayerSeen && !patrolling && isPatrol) 
            StartCoroutine(LostPlayer(Random.Range(2, 4)));

        //Если ИИ видит игрока
        if (isPlayerSeen)
        {
            ChaseTarget(currentTarget);
        }
        //Если ИИ не видит игрока, но он патрульный
        else if (patrolling)
        {
            PatrolArea();
        }

        //Если ИИ ничего не видит, и направление его пути не равно нулю, то направление его зрения должно быть равно направлению его движения
        if (!isPlayerSeen && ai.velocity != Vector3.zero) direction = ai.velocity.normalized;

        //Отправляем контроллеру анимаций данные передвижениях ИИ
        anim.isWalk = !ai.reachedEndOfPath;
        anim.direction = direction;
    }

    private void PatrolArea()
    {
        ai.maxSpeed = patrolSpeed; //Меняем скорость на обычную

        //Если ИИ достиг конца пути, не ищет путь и его время ожидания не равно бесконечности, 
        //то мы назначаем ему время после которого ему надо будет идти к другой точке патруля
        if (ai.reachedEndOfPath && !ai.pathPending && float.IsPositiveInfinity(swTime))
        {
            swTime = Random.Range(.5f, 6f);
            currTime = 0f;
        }


        //Если пора искать путь
        if (currTime >= swTime)
        {
            if((Vector2) ai.destination == path[currWay]){
                currWay++; //Обновляем путь
                currWay %= path.Length; //Вычисляем остаток от деления текущего пути на длины массива путей, чтобы текущий путь не превышал кол-во путей
            }
            
            ai.destination = path[currWay]; //Назначаем ИИ путь
            ai.SearchPath(); //Если надо ищем этот самый путь
            swTime = float.PositiveInfinity; //Делаем время ожидания недосигаемым
        }

        currTime += Time.deltaTime; //Time.time неюзабилен после 9ти часов работы приложения, поэтому заменил на вариант с deltaTime
    }

    private void ChaseTarget(Transform target)
    {
        patrolling = false; //Отключаем патруль
        swTime = float.PositiveInfinity;
        currentTarget = target;
        ai.maxSpeed = spotSpeed; //Меняем ему скорость
        ai.destination = target.position; //Меняем ему цель на объект
        ai.SearchPath(); //Ищем путь до объекта
    }

    //Система реагирования на скриптовые шумы
    public void NoiseDistraction(Transform noise)
    {
        ChaseTarget(noise);
    }

    //Функция начала путрлирования, после того как ИИ некоторое время не видел игрока
    IEnumerator LostPlayer(float timeTillPatrolling)
    {
        yield return new WaitForSeconds(timeTillPatrolling);
        patrolling = true;
    }

    //Для наглядности рисуем точки патруля желтыми кружками
    void OnDrawGizmos() 
    {
        //Границы зрения
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 0f, viewAngle/2) * direction * seeRange);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 0f, -viewAngle/2) * direction * seeRange);

        if (isPatrol)
        {
            Gizmos.color = Color.green;
            Vector2 lastPoint = path[0];
            foreach (var currentPoint in path)
            {
                Gizmos.DrawLine(lastPoint,currentPoint);
                Gizmos.DrawWireSphere(currentPoint, .2f);
                lastPoint = currentPoint;
                Gizmos.color = Color.yellow;
            }
            Gizmos.DrawLine(lastPoint, path[0]);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(lastPoint, .2f);
        }
    }
}
