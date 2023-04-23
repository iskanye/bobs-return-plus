using UnityEngine;

public class RatKingCheese : ActionBase
{
    public void Distract(GameObject rat)
    {
        if (rat.transform.parent != null && rat.transform.parent.TryGetComponent<RatKing>(out var ratKing))
            ratKing.Distract(action);
    }
}
