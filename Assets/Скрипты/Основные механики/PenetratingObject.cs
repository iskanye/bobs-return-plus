using UnityEngine;

public class PenetratingObject : MonoBehaviour
{
    public ObjectType type;
    public Vector3 particlesOffset;
    public float particlesForce;
    public Rigidbody2D[] destroyParticles;
    public UnityEngine.Events.UnityEvent<GameObject> onPenetrate;

    public ObjectType Durability { set => type = value; }

    public void Penetrate() 
    {
        if (destroyParticles != null)
            foreach (var i in destroyParticles)
                Instantiate(i, transform.position + particlesOffset, Quaternion.identity).AddForce(particlesForce * (Vector2)Random.onUnitSphere, ForceMode2D.Impulse);

        onPenetrate.Invoke(gameObject);
    }
}
