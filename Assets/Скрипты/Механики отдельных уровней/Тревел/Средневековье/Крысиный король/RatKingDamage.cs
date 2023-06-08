using UnityEngine;

public class RatKingDamage : ActionBase
{
    public int ratsKilling = 1;
    
    public void Damage(GameObject rat)
    {
        if (rat.transform.parent != null && rat.transform.parent.TryGetComponent<RatKing>(out var ratKing))
        {
            action?.Invoke(ratKing.gameObject);
            ratKing.KillRats(ratsKilling);
        }
    }
}
