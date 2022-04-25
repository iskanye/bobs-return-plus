using System.Collections;

[UnityEngine.CreateAssetMenu(fileName = "Slowdown Effect", menuName = "Effects/Slowdown", order = 0)]
public class Slowdown : PlayerEffectBase
{
    float prevSpeed;

    public override IEnumerator Start()
    {
        prevSpeed = data.movement.speed;
        data.movement.speed -= 2;

        yield return base.Start();
    }

    public override IEnumerator Stop()
    {
        data.movement.speed = prevSpeed;
        yield return base.Stop();
    }
}
