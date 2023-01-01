using System.Collections;

[UnityEngine.CreateAssetMenu(fileName = "Speed Change Effect", menuName = "Effects/Speed Change", order = 0)]
public class SpeedChange : PlayerEffectBase
{
    public bool isSlowdown;

    float prevSpeed;

    public override IEnumerator Start()
    {
        prevSpeed = data.movement.speed;
        data.movement.speed += isSlowdown ? -2 : 2;

        yield return base.Start();
    }

    public override IEnumerator Stop()
    {
        data.movement.speed = prevSpeed;
        yield return base.Stop();
    }
}
