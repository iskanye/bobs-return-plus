using UnityEngine;

public class ZombieHead : MonoBehaviour
{
    public Vector3 explosionOffset;
    public Rigidbody2D[] lumps;
    public GameObject bloodExplosion;
    public GameObject bloodPuddle;

    void OnCollisionEnter2D(Collision2D c) 
    {
        Instantiate(bloodExplosion, transform.position + explosionOffset, Quaternion.identity);
        Instantiate(bloodPuddle, transform.position + explosionOffset, Quaternion.identity);

        foreach (var i in lumps) 
        {
            var lump = Instantiate(i, transform.position, Quaternion.identity);
            lump.velocity = (Vector2)Random.onUnitSphere * 3;
        }

        Destroy(gameObject);
    }
}
