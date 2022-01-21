using UnityEngine;

public class Damageable : MonoBehaviour
{
    public LiveCounter player;

    public void Damage() => player.LoseLife();
}
