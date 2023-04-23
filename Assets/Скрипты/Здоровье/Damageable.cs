using UnityEngine;

public class Damageable : ActionBase
{
    public int damage = 1;
    public bool isDeadly;
    public bool addPersistentListener;
    public bool directionBasedOnVelocity;
    public ObjectType penetrating = ObjectType.Fragile;
    public bool onlyThisPenetratingType;
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
        if (g.TryGetComponent<LivesBase>(out var lives) && ((lives.Durability <= penetrating && !onlyThisPenetratingType) 
            || (lives.Durability == penetrating && onlyThisPenetratingType)))
        {
            if (TryGetComponent<Rigidbody2D>(out var rigid) && directionBasedOnVelocity)
                direction = rigid.velocity.normalized;

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

            action?.Invoke(g);
        }

        if (g.TryGetComponent<PenetratingObject>(out var penetr) && ((penetr.type <= penetrating && !onlyThisPenetratingType) 
            || (penetr.type == penetrating && onlyThisPenetratingType)))
        {
            penetr.Penetrate();
            action?.Invoke(g);
        }
    }
}