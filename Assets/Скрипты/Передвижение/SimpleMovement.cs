using UnityEngine;

public class SimpleMovement : BaseMovement
{
    public Vector2 endPoint;
    public float startDelay;
    public bool repeat;
    public float repeatDelay;

    [HideInInspector] public Vector2 startPoint;

    MoveState moveState;
    State<SimpleMovement> state;

    void Awake()
    {
        startPoint = transform.localPosition;
        moveState = new MoveState(this);

        if (startDelay >= 0)
            Invoke("StartMove", startDelay);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, (Vector3)endPoint + transform.position);
        Gizmos.DrawWireSphere((Vector3)endPoint + transform.position, .1f);
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
        ChangeState(moveState);
}

public class MoveState : State<SimpleMovement> 
{ 
    public MoveState(SimpleMovement mn) : base(mn) { }

    float t;

    public override System.Collections.IEnumerator Update() 
    {
        mn.endPoint += (Vector2)mn.transform.localPosition;
        mn.IsWalking = true;

        while (true) 
        {
            mn.Direction = (mn.endPoint - mn.startPoint).normalized;
            mn.transform.localPosition = Vector2.Lerp(mn.startPoint, mn.endPoint, t);

            if (t >= 1) 
            {
                if (mn.repeat)
                {
                    yield return new WaitForSeconds(mn.repeatDelay);
                    t = 0;
                    (mn.startPoint, mn.endPoint) = (mn.endPoint, mn.startPoint);
                }
                else
                {
                    mn.ChangeState(null);
                    mn.IsWalking = false;
                }
            }

            t += mn.speed * Time.deltaTime;
            yield return base.Update();
        }
    }
}
