using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int damage = 1;
    public bool isDeadly;
    public bool addPersistentListener;
    public ObjectType penetrating = ObjectType.Fragile;
    public System.Action<int> onDamage;

    [HideInInspector] public DiscardingType discarding = DiscardingType.Small;
    [HideInInspector] public Vector2 direction;

    void Awake()
    {
        if (addPersistentListener)
            GetComponent<Trigger>().action.AddListener(Damage);
    }

    public void Damage(GameObject g)
    {
        var lives = g.GetComponent<LivesBase>();

        if (lives)
        {
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