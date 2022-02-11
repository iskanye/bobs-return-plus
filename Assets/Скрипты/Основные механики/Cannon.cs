using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject bullet;
    public float distance;
    public Vector2 direction;
    public LayerMask player;
    public float force;
    public bool isReloadable;
    public float reloadDelay;
    public bool HaveShoted { set; get; }

    float swTime;
    PropertyHolder prop;

    void Awake() =>
        prop = GetComponent<PropertyHolder>();
    
    void Update()
    {
        if (Physics2D.Raycast(transform.position, direction, distance, player))
        {
            if (!isReloadable && !HaveShoted)
            {
                var bull = Instantiate(bullet, transform.position, Quaternion.identity);
                bull.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
                HaveShoted = true;

                if (prop != null) 
                    prop.property = true;
            }

            else if (isReloadable)
            {
                if (float.IsPositiveInfinity(swTime)) 
                    swTime = Time.time + reloadDelay;

                if (Time.time >= swTime)
                {
                    var bull = Instantiate(bullet, transform);
                    bull.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse); 
                    swTime = float.PositiveInfinity;                  
                }
            }
        }

        else    
            swTime = float.PositiveInfinity;
    }
}
