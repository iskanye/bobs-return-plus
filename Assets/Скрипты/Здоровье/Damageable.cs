public class Damageable : UnityEngine.MonoBehaviour
{
    public void Damage() => LiveCounter.LoseLife();
}
