using UnityEngine;
using Pathfinding;

//Скрипт ИИ
public class AI : MonoBehaviour
{
    public LayerMask obstacles, player; //Слой, видимый для ИИ
    //public float speed; //Скорость
    public float seeRange; //Дальность зрения
    public float spotSpeed;    
    public Vector2 direction = Vector2.right;
    [Range(0, 360)] public float viewAngle;
    [Header("Patrol AI")] public bool isPatrol; //Является ли данный ИИ патрульным     
    public float patrolSpeed;   
    public Vector2[] path; //Массив пути(для патрульного ИИ)
    [HideInInspector] public bool isSee; //Видит ли ИИ игрока?
    [HideInInspector] public Vector2 target; //Позиция цели

    AILerp ai; //Скрипт поиска пути
    RaycastHit2D[] hit = new RaycastHit2D[31]; //Лучи(зрение ИИ)
    bool patr; //Патрулирует ли ИИ?
    int currWay = 0; //Текущий путь из массива позиций
    float swTime1; //Время ожидания(для патруля)

    void Awake()
    {
        //Получаем скрипт поиска пути
        ai = GetComponent<AILerp>();
        patr = isPatrol;
    }

    void Update()
    {
        var check = Physics2D.OverlapCircle(transform.position, seeRange, player);

        if (check != null)
        {
            if (Mathf.Acos(Vector2.Dot(direction, (check.transform.position - transform.position).normalized)) * Mathf.Rad2Deg < viewAngle * .5f)
            {
                if (!Physics2D.Raycast(transform.position, (check.transform.position - transform.position).normalized,
                (check.transform.position - transform.position).magnitude, obstacles))
                {             
                    target = check.transform.position;
                    direction = (check.transform.position - transform.position).normalized;
                    isSee = true;
                }
                else isSee = false;
            }

            else isSee = false;
        }

        else if (isSee) isSee = false;

        if (ai.reachedEndOfPath && !isSee && !patr && isPatrol) Invoke("StartPatroling", Random.Range(2, 4));

        if (isSee)
        {
            patr = false;
            ai.speed = spotSpeed;
            currWay = 0;            
            swTime1 = float.PositiveInfinity;
            ai.destination = target;
            ai.SearchPath();
        }

        else if (patr)
        {
            ai.speed = patrolSpeed;
            var search = false;

            if (ai.reachedEndOfPath && !ai.pathPending && float.IsPositiveInfinity(swTime1)) swTime1 = Time.time + Random.Range(.5f, 6);

            if (Time.time >= swTime1)
            {
                currWay++;
                search = true;
                swTime1 = float.PositiveInfinity;
            }

            currWay %= path.Length;
            ai.destination = path[currWay];

            if (search) ai.SearchPath();

        }

        transform.localScale = new Vector2(ai.velocity.x < 0 ? -1 : 1, transform.localScale.y);
        if (!isSee && ai.velocity != Vector3.zero) direction = ai.velocity.normalized;
    }

    public void NoiseDistraction(Vector2 noise)
    {
        patr = false;        
        ai.speed = spotSpeed;  
        ai.destination = noise;     
        ai.SearchPath();
    }

    void StartPatroling() => patr = true;
}
