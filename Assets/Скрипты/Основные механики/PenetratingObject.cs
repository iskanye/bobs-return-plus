using UnityEngine;

public class PenetratingObject : MonoBehaviour
{
    public ObjectType type;
    public GameObject destroyParticles;
    public Destroy destroy;

    public ObjectType Durability { set => type = value; }

    public void Penetrate() 
    {
        Instantiate(destroyParticles, transform.position, Quaternion.identity);
        destroy.Do();
    }
}
