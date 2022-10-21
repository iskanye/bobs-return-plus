using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject bullet;
    public float distance;
    public Vector2 direction;
    public Vector2 bulletOffset;
    public Vector2 offset;
    public LayerMask player;
    public float force;
    public bool isReloadable;
    public bool rotating;
    public float reloadDelay;
    public Animator[] animators;

    public bool HaveShoted { set; get; }

    float swTime;
    PropertyHolder prop;

    void Awake() =>
        prop = GetComponent<PropertyHolder>();
    
    void Update()
    {
        foreach (var i in animators)
        {
            i.SetFloat("DirX", direction.x);
            i.SetFloat("DirY", direction.y);
        }

        if (Physics2D.Raycast(transform.position + (Vector3)offset, direction, distance, player))
        {
            if (!isReloadable && !HaveShoted)
            {
                var bull = Instantiate(bullet, transform.position + (Vector3)bulletOffset, rotating ? Quaternion.Euler(direction) : Quaternion.identity);
                bull.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
                HaveShoted = true;

                foreach (var i in animators)
                    i.SetTrigger("Shoot");

                if (prop != null)
                    prop.property = true;
            }

            else if (isReloadable && float.IsPositiveInfinity(swTime))
            {
                var bull = Instantiate(bullet, transform.position + (Vector3)bulletOffset, rotating ? Quaternion.Euler(direction) : Quaternion.identity);
                bull.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
                swTime = Time.time + reloadDelay;

                foreach (var i in animators)
                    i.SetTrigger("Shoot");
            }
        }

        if (Time.time >= swTime)
            swTime = float.PositiveInfinity;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + (Vector3)bulletOffset, .1f);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + (Vector3)offset, direction * distance);
    }
}
