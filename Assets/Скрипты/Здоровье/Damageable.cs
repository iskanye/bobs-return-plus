using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int damage = 1;
    public bool isDeadly;
    public System.Action<int> onDamage;

    [HideInInspector] public DiscardingType discarding;
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public ObjectType penetrating;

    public void Damage(GameObject g)
    {
        var obj = g.GetComponent<LivesBase>();

        if (obj != null)
            if (isDeadly)
            {
                obj.Lives = 0;
                onDamage?.Invoke(0);
            }

            else
            {
                onDamage?.Invoke(obj.Lives - damage);
                obj.Lives -= damage;
            }

        var rigid = g.GetComponent<Rigidbody2D>();

        if (rigid != null)
            rigid.velocity += direction * (int)discarding;

        var penetr = g.GetComponent<PenetratingObject>();

        if (penetr != null && penetr.type <= penetrating)
            penetr.Penetrate();
    }
}