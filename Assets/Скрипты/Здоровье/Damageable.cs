public class Damageable : UnityEngine.MonoBehaviour
{
    public void Damage() => LiveCounter.Active.LoseLife();
}