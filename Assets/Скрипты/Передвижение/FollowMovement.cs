using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FollowMovement : BaseMovement
{
    public Transform target;
    public float stopRadius;

    Rigidbody2D rigid;

    void Awake() =>
        rigid = GetComponent<Rigidbody2D>();

    public void FixedUpdate()
    {
        var direction = (target.position - gameObject.transform.position);

        if (direction.magnitude >= stopRadius)
        {
            IsWalking = true;
            rigid.velocity = direction.normalized * speed;
            Direction = direction.normalized;
        }      
        else 
        {
            IsWalking = false;
            rigid.velocity = Vector2.zero;
        }
    }
}