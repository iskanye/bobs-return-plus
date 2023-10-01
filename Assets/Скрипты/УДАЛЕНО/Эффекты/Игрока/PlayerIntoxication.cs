using UnityEngine;

[CreateAssetMenu(fileName = "Player Intoxication Effect", menuName = "Effects/Player/Intoxication", order = 0)]
public class PlayerIntoxication : PlayerEffectBase
{
    public override System.Collections.IEnumerator Process()
    {
        while (true)
        {
            if (data.lives.Lives > 1)
                data.lives.Lives -= 1;

            if (data.lives.Lives == 1)
                mn.StartCoroutine(Stop());

            yield return new WaitForSeconds(2);
        }
    }
}

