using UnityEngine;

public class Damageable : MonoBehaviour
{
    public void Damage(GameObject g) =>
        g.GetComponent<ILives>().Lives--;
}