using UnityEngine;

[CreateAssetMenu(fileName = "Player Nausea Effect", menuName = "Effects/Player/Nausea", order = 0)]
public class PlayerNausea : PlayerEffectBase
{
    public override System.Collections.IEnumerator Start()
    {
        CameraController.Active.StartShake(.1f, duration);
        yield return base.Start();
    }
}