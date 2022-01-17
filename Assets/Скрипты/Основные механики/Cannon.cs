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

    float swTime;
    bool haveShoted;

    void Update()
    {
        if (Physics2D.Raycast(transform.position, direction, distance, player))
        {
            if (!isReloadable && !haveShoted)
            {
                var bull = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
                bull.AddForce(direction * force, ForceMode2D.Impulse);
                haveShoted = true;
            }

            else if (isReloadable)
            {
                if (float.IsPositiveInfinity(swTime)) swTime = Time.time + reloadDelay;

                if (Time.time >= swTime)
                {
                    var bull = Instantiate(bullet, transform).GetComponent<Rigidbody2D>();
                    bull.AddForce(direction * force, ForceMode2D.Impulse);  
                    swTime = float.PositiveInfinity;                  
                }
            }
        }

        else swTime = float.PositiveInfinity;
    }
}
