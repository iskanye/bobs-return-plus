using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int damage = 1;
    public bool isDeadly;
    public bool addPersistentListener;
    public bool directionBasedOnVelocity;
    public ObjectType penetrating = ObjectType.Fragile;
    public System.Action<int> onDamage;
    public DiscardingType discarding = DiscardingType.Small;
    public Vector2 direction;

    void Awake()
    {
        if (addPersistentListener)
            GetComponent<Trigger>().action.AddListener(Damage);
    }

    public void Damage(GameObject g)
    {
        var lives = g.GetComponent<LivesBase>();

        if (lives && lives.Durability <= penetrating)
        {
            var rigid = GetComponent<Rigidbody2D>();

            if (rigid && directionBasedOnVelocity)
                direction = rigid.velocity;

            lives.hitDirection = direction * (int)discarding;

            if (isDeadly)
            {
                onDamage?.Invoke(lives.Lives);
                lives.Lives = 0;
            }

            else
            {
                onDamage?.Invoke(damage);
                lives.Lives -= damage;
            }
        }

        var penetr = g.GetComponent<PenetratingObject>();

        if (penetr && penetr.type <= penetrating)
            penetr.Penetrate();
    }
}