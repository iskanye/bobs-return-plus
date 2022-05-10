using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int damage = 1;
    public bool isDeadly;

    public void Damage(GameObject g)
    {
        var obj = g.GetComponent<LivesBase>();

        if (obj != null)
            if (isDeadly)
                obj.Lives = 0;

            else
                obj.Lives -= damage;
    }
}