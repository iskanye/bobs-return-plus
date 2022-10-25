using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float distance;
    [SerializeField] private Vector2 direction;
    [SerializeField] private Vector2 bulletOffset;
    [SerializeField] private Vector2 offset;
    [SerializeField] private LayerMask player;
    [SerializeField] private float force;
    [SerializeField] private bool isReloadable;
    [SerializeField] private float reloadDelay;
    [SerializeField] private Animator[] animators;

    private bool haveShot;
    
    private float swTime;
    private PropertyHolder prop;
    
    private static readonly int Shoot = Animator.StringToHash("Shoot");
    private static readonly int DirX = Animator.StringToHash("DirX");
    private static readonly int DirY = Animator.StringToHash("DirY");
    
    private BoxCollider2D trigger;
    
    void Awake()
    {
        prop = GetComponent<PropertyHolder>();
        foreach (var i in animators)
        {
            i.SetFloat(DirX, direction.x);
            i.SetFloat(DirY, direction.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if ((1<<col.gameObject.layer) != player.value)
            return;
        
        bool raycastHit2D = Physics2D.Raycast(transform.position + (Vector3)offset, direction, distance, player);
        if (raycastHit2D)
        {
            if (!haveShot || (Time.time >= swTime && isReloadable))
            {
                var bull = Instantiate(bullet, transform.position + (Vector3)bulletOffset, Quaternion.identity);
                bull.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
                haveShot = true;
                swTime = Time.time + reloadDelay;
                
                foreach (var i in animators)
                    i.SetTrigger(Shoot);

                if (prop != null)
                    prop.property = true;
            }
        }
    }

    private void OnValidate()
    {
        trigger = GetComponent<BoxCollider2D>();
        trigger.offset = offset + Vector2.right*distance/2;
        trigger.size = new Vector2(distance, 0.01f);
        trigger.isTrigger = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + (Vector3)bulletOffset, .1f);
    }
}
