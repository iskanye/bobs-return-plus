using UnityEngine;

[CreateAssetMenu(fileName = "Player Healing Effect", menuName = "Effects/Player/Healing", order = 0)]
public class PlayerHealing : PlayerEffectBase
{
    public override System.Collections.IEnumerator Process()
    {
        while (true) 
        {
            data.lives.Lives += 1;
            yield return new WaitForSeconds(2);
        }
    }
}
