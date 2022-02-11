using UnityEngine;

public class MoveToAndBack : MonoBehaviour
{
    public Vector2 endPoint;
    [Range(0, 10)] public float speed = .5f;
    public float startDelay;
    public bool repeat;

    [HideInInspector] public Vector2 startPoint;

    MoveState moveState;
    State<MoveToAndBack> state;

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

    public void ChangeState(State<MoveToAndBack> state) 
    {
        if (this.state != null)
            StartCoroutine(this.state.Stop());

        this.state = state;
        if (this.state != null) StartCoroutine(this.state.Start());
    }

    public void StartMove() =>
        ChangeState(moveState);
}

public class MoveState : State<MoveToAndBack> 
{ 
    public MoveState(MoveToAndBack mn) : base(mn) { }

    float t;

    public override System.Collections.IEnumerator Update() 
    { 
        while (true) 
        {
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
                    mn.ChangeState(null);
            }

            yield return base.Update();
        }
    }
}