using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SimpleMovement : BaseMovement
{
    public Vector2[] path = new Vector2[1];
    public float startDelay = -1;
    public float delay;
    public bool repeat;
    public float changePointRadius = .1f;

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
        var prevPoint = transform.position;

        for (int i = 0; i < path.Length; i++) 
        {       
            Gizmos.DrawLine(prevPoint, path[i]);
            Gizmos.DrawWireSphere(path[i], .1f);
            prevPoint = path[i];
        }

        if (repeat)
            Gizmos.DrawLine(path[^1], path[0]);
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

    public override IEnumerator Update() 
    {
        mn.IsWalking = true;        
        Vector2 prevPoint = mn.transform.position;
        var currPoint = 0;

        while (true) 
        {
            mn.Direction = (mn.path[currPoint] - prevPoint).normalized;
            mn.rigid.velocity = mn.Direction * mn.speed;

            if ((mn.path[currPoint] - (Vector2)mn.transform.position).magnitude <= mn.changePointRadius) 
            {
                prevPoint = mn.path[currPoint];
                currPoint++;
                
                mn.IsWalking = false;
                yield return new WaitForSeconds(mn.delay);
                mn.IsWalking = true;
            }

            if (currPoint == mn.path.Length)
            {
                if (mn.repeat)
                    currPoint = 0;

                else
                {
                    mn.rigid.velocity = Vector2.zero;
                    mn.IsWalking = false;
                    mn.StopMove();
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
