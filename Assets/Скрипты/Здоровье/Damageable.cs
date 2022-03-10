using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int damage = 1;

    public void Damage(GameObject g) =>
        g.GetComponent<ILives>().Lives -= damage;
}