using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SimpleMovement : BaseMovement
{
    public Vector2 endPoint;
    public float startDelay = -1;
    public bool repeat;
    public float repeatDelay;
    public float stopRadius = .1f;

    [HideInInspector] public Vector2 startPoint;
    [HideInInspector] public Rigidbody2D rigid;

    MoveState moveState;
    State<SimpleMovement> state;

    void Awake()
    {
        startPoint = transform.position;
        rigid = GetComponent<Rigidbody2D>();
        moveState = new MoveState(this);

        if (startDelay >= 0)
            Invoke("StartMove", startDelay);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, (Vector3)endPoint);
        Gizmos.DrawWireSphere((Vector3)endPoint, .1f);
    }

    public void ChangeState(State<SimpleMovement> state) 
    {
        if (this.state != null)
            StartCoroutine(this.state.Stop());

        this.state = state;

        if (this.state != null)
            StartCoroutine(this.state.Start());
    }

    public void StartMove() =>
        ChangeState(moveState);

    public void StopMove() =>
        ChangeState(null);
}

public class MoveState : State<SimpleMovement> 
{ 
    public MoveState(SimpleMovement mn) : base(mn) { }

    public override System.Collections.IEnumerator Update() 
    {
        mn.IsWalking = true;

        while (true) 
        {
            mn.Direction = (mn.endPoint - (Vector2)mn.transform.position).normalized;
            mn.rigid.velocity = mn.Direction * mn.speed;

            if ((mn.endPoint - (Vector2)mn.transform.position).magnitude <= mn.stopRadius) 
            {
                if (mn.repeat)
                {
                    mn.IsWalking = false;
                    yield return new WaitForSeconds(mn.repeatDelay);
                    mn.IsWalking = true;

                    (mn.startPoint, mn.endPoint) = (mn.endPoint, mn.startPoint);
                }
                else
                {
                    mn.rigid.velocity = Vector2.zero;
                    mn.StopMove();
                    mn.IsWalking = false;
                }
            }

            yield return base.Update();
        }
    }
}
