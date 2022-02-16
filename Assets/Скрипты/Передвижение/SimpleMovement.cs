using UnityEngine;

public class SimpleMovement : MonoBehaviour, IWalkable
{
    public Vector2 endPoint;
    [Range(0, 10)] public float speed = .5f;
    public float startDelay;
    public bool repeat;

    public bool IsWalking { get; set; }
    public Vector2 Direction { get; set; }

    [HideInInspector] public Vector2 startPoint;

    MoveState moveState;
    State<SimpleMovement> state;

    void Awake()
    {
        startPoint = gameObject.transform.position;
        moveState = new MoveState(this);

        if (startDelay >= 0)
            Invoke("StartMove", startDelay);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(gameObject.transform.position, endPoint);
        Gizmos.DrawWireSphere(endPoint, .25f);
    }

    public void ChangeState(State<SimpleMovement> state) 
    {
        if (state != null)
            StartCoroutine(this.state.Stop());

        this.state = state;

        if (state != null)
            StartCoroutine(this.state.Start());
    }

    public void StartMove() =>
        ChangeState(moveState);
}

public class MoveState : State<SimpleMovement> 
{ 
    public MoveState(SimpleMovement mn) : base(mn) { }

    float t;

    public override System.Collections.IEnumerator Update() 
    {
        mn.IsWalking = true;

        while (true) 
        {
            mn.Direction = (mn.endPoint - mn.startPoint).normalized;

            mn.gameObject.transform.position = Vector2.Lerp(mn.startPoint, mn.endPoint, t);
            t += mn.speed * Time.deltaTime;

            if (t >= 0) 
            {
                if (mn.repeat)
                {
                    t = 0;
                    (mn.startPoint, mn.endPoint) = (mn.endPoint, mn.startPoint);
                }
                else
                {
                    mn.ChangeState(null);
                    mn.IsWalking = false;
                }
            }

            yield return base.Update();
        }
    }
}
