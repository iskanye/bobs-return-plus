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
        var lives = g.GetComponent<LivesBase>();

        if (lives != null)
            if (isDeadly)
            {
                onDamage?.Invoke(lives.Lives);
                lives.Lives = 0;
            }

            else
            {
                onDamage?.Invoke(lives.Lives - damage);
                lives.Lives -= damage;
            }

        var rigid = g.GetComponent<Rigidbody2D>();

        if (rigid != null)
            rigid.velocity += direction * (int)discarding;

        var penetr = g.GetComponent<PenetratingObject>();

        if (penetr != null && penetr.type <= penetrating)
            penetr.Penetrate();

        var bob = g.GetComponent<BobController>();

        if (bob != null)
        {
            bob.enabled = false;
            bob.enabled = true;
        }
    }
}