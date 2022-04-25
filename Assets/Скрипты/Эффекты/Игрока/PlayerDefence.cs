using System.Collections;

[UnityEngine.CreateAssetMenu(fileName = "Defence Effect", menuName = "Effects/Player/Defence", order = 0)]
public class PlayerDefence : PlayerEffectBase
{
    int LivesCalculation(int lives)
    {
        if (data.lives.Lives > lives)        
            return lives + 1;

        return lives;
    }

    public override IEnumerator Start()
    {
        data.lives.livesCalculation += LivesCalculation;
        yield return base.Start();
    }

    public override IEnumerator Stop()
    {
        data.lives.livesCalculation -= LivesCalculation;
        yield return base.Stop();
    }
}
