[UnityEngine.CreateAssetMenu(fileName = "Player Death Effect", menuName = "Effects/Player/Death", order = 0)]
public class PlayerDeath : PlayerEffectBase
{
    public override System.Collections.IEnumerator Stop()
    {
        data.lives.Lives = 0;
        yield return base.Stop();
    }
}
