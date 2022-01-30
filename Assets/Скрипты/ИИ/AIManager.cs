using UnityEngine;
using System.Collections;
using Pathfinding;
using AI;
using System;

//Скрипт ИИ
public class AIManager : MonoBehaviour, IWalkable
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

    private AnimationMovementController anim; //Контроллер анимаций
    private IAstarAI ai; //Скрипт поиска пути
    public AnimationMovementController Anim => anim;
    public IAstarAI AI => ai;

    public bool IsWalking => AI.velocity != Vector3.zero;

    public Vector2 Direction => direction;

    [HideInInspector] public int currWay = 0; //Текущий путь из массива позиций
    [HideInInspector] public Transform currentTarget; //Трансформ цели

    //States
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public ChaseState chaseState;
    [HideInInspector] public SearchState searchState;
    [SerializeField] private AIState currentState;


    void Awake()
    {
        //Получаем скрипт поиска пути и контроллер анимаций
        ai = GetComponent<AILerp>();
        anim = GetComponent<AnimationMovementController>();

        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);
        searchState = new SearchState(this);

        ChangeState(searchState);
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

    //Система реагирования на скриптовые шумы
    public void NoiseDistraction(Transform noise)
    {
        ChaseTarget(noise);
    }

    public void ChangeState(AIState state)
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
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 0f, viewAngle / 2) * direction * seeRange);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 0f, -viewAngle / 2) * direction * seeRange);

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


