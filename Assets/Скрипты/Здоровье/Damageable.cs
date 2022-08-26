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
                onDamage?.Invoke(lives.Lives - damage);
                lives.Lives -= damage;
            }
        }

        var penetr = g.GetComponent<PenetratingObject>();

        if (penetr && penetr.type <= penetrating)
            penetr.Penetrate();

        var bob = g.GetComponent<BobController>();

        if (bob)
        {
            bob.enabled = false;
            bob.enabled = true;
        }
    }
}