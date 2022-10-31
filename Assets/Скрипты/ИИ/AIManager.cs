using UnityEngine;
using AI;
using System;
using Pathfinding;

//Скрипт ИИ
public class AIManager : BaseMovement, IInteger
{
    public LayerMask obstacles = 1 << 3; //Слой с препятствиями
    public LayerMask player = 1 << 6; //Слой с игроком
    public float seeRange; //Дальность зрения
    public float spotSpeed; //Скорость движения(При погоне)
    public float searchSpeed = .5f;
    public Vector2 direction = Vector2.right;
    [Range(0, 360)] public float viewAngle;

    [Header("Patrol AI")] public bool isPatrol; //Является ли данный ИИ патрульным     
    public float patrolDelay;
    public Vector2[] path; //Массив пути(для патрульного ИИ)

    public IAstarAI AI { get; private set; }

    public int integer
    {
        get =>
            currWay - 1;

        set =>
            currWay = value;
    }

    public bool canMove { get => AI.canMove; set => AI.canMove = value; }

    public override bool IsWalking => AI.velocity != Vector3.zero && canMove;

    [HideInInspector] public int currWay; //Текущий путь из массива позиций
    [HideInInspector] public Transform currentTarget; //Трансформ цели

    //States
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public ChaseState chaseState;
    [HideInInspector] public SearchState searchState;
    [HideInInspector] public IdleState idleState;

    State<AIManager> currentState;

    void Awake()
    {
        //Получаем скрипт поиска пути и контроллер анимаций
        AI = GetComponent<IAstarAI>();

        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);
        searchState = new SearchState(this);
        idleState = new IdleState(this);

        ChangeState(searchState);
    }

    void OnEnable()
    {
        canMove = true;
        ChangeState(searchState);
    }

    void Update()
    {
        if (direction != Vector2.zero)
        {
            float x = Mathf.Round(direction.x);
            float y = Mathf.Round(direction.y);
            Direction = new Vector2(x, x == y ? 0 : y);
        }
    }

    void OnDisable()
    {
        StopAllCoroutines();
        canMove = false;
    }

    public bool CanSeePlayer()
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

    public void ChaseTarget(Transform target)
    {
        currentTarget = target;
        ChangeState(chaseState);
    }

    public void ChaseTarget(GameObject target) =>
        ChaseTarget(target.transform);

    //Система реагирования на скриптовые шумы
    public void NoiseDistraction(Transform noise) =>
        ChaseTarget(noise);

    public void ChangeState(State<AIManager> state)
    {
        if (currentState != null)
            StartCoroutine(chaseState.Stop());

        currentState = state;
        StartCoroutine(currentState.Start());
    }

    //Для наглядности рисуем точки патруля желтыми кружками
    void OnDrawGizmos()
    {
        //Границы зрения
        Gizmos.color = Color.white;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 0f, viewAngle / 2) * direction * seeRange);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 0f, -viewAngle / 2) * direction * seeRange);
        Gizmos.DrawRay(transform.position, direction * seeRange);

        if (isPatrol)
        {
            Gizmos.color = Color.green;
            Vector2 lastPoint = path[0];

            foreach (var currentPoint in path)
            {
                Gizmos.DrawLine(lastPoint, currentPoint);
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
