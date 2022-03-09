public class Damageable : UnityEngine.MonoBehaviour
{
    public void Damage() => 
        LiveCounter.Active.LoseLifes(1);

    public void Damage(int damage) => 
        LiveCounter.Active.LoseLifes(damage);
}