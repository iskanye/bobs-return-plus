using UnityEngine;

public class Damageable : MonoBehaviour
{
    public bool isDeadly;

    public void Damage(GameObject g)
    {
        var obj = g.GetComponent<ILives>();

        if (obj != null)
            if (isDeadly)
                obj.Lives = int.MinValue;

            else
                obj.Lives--;
    }
}