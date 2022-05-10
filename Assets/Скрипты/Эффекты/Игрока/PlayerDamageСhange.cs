using System.Collections;

[UnityEngine.CreateAssetMenu(fileName = "Player Damage Сhange Effect", menuName = "Effects/Player/Damage Сhange", order = 0)]
public class PlayerDamageСhange : PlayerEffectBase
{
    public bool isWeakening;

    int prevDamage;

    public override IEnumerator Start()
    {
        prevDamage = data.damageable.damage;
        data.damageable.damage = (int)(data.damageable.damage * (isWeakening ? 0.75f : 1.25f));
        yield return base.Start();
    }

    public override IEnumerator Stop()
    {
        data.damageable.damage = prevDamage;
        yield return base.Stop();
    }
}
