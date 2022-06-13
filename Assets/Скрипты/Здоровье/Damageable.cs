using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int damage = 1;
    public bool isDeadly;
    public System.Action<int> onDamage;

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
    }
}