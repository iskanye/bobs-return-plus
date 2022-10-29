using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Cannon : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] DiscardingType bulletDiscarding;
    [SerializeField] Vector2 detectionArea;
    [SerializeField] Vector2Int direction;
    [SerializeField] Vector2 bulletOffset;
    [SerializeField] Vector2 offset;
    [SerializeField] LayerMask player;
    [SerializeField] float force;
    [SerializeField] bool isReloadable;
    [SerializeField] float reloadDelay;
    [SerializeField] Animator animator;

    bool haveShot;
    
    float shootTime;
    PropertyHolder prop;
    
    static readonly int shoot = Animator.StringToHash("Shoot");
    static readonly int dirX = Animator.StringToHash("DirX");
    static readonly int dirY = Animator.StringToHash("DirY");
    
    BoxCollider2D trigger;
    
    void Awake()
    {
        prop = GetComponent<PropertyHolder>();

        animator.SetFloat(dirX, direction.x);
        animator.SetFloat(dirY, direction.y);
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if ((1 << c.gameObject.layer) != player)
            return;

        if (!haveShot || (Time.time >= shootTime && isReloadable))
        {
            var bull = Instantiate(bullet, transform.position + (Vector3)bulletOffset, bullet.transform.rotation);
            bull.GetComponent<Rigidbody2D>().AddForce((Vector2)direction * force, ForceMode2D.Impulse);

            foreach (var i in bull.GetComponents<Damageable>()) 
            {
                i.direction = direction;
                i.discarding = bulletDiscarding;
            }

            haveShot = true;
            shootTime = Time.time + reloadDelay;
            animator.SetTrigger(shoot);

            if (prop != null)
                prop.property = true;
        }
    }

    void OnValidate()
    {
        trigger = GetComponent<BoxCollider2D>();
        trigger.offset = offset + (Vector2)direction * detectionArea.x / 2;
        trigger.size = direction.x != 0 ? new Vector2(detectionArea.x, detectionArea.y) : new Vector2(detectionArea.y, detectionArea.x);
        trigger.isTrigger = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + (Vector3)bulletOffset, .1f);
    }
}
