using UnityEngine;

public class RatKingCheese : ActionBase
{
    public float time = 5;

    public void Distract(GameObject rat)
    {
        if (rat.transform.parent != null && rat.transform.parent.TryGetComponent<RatKing>(out var ratKing))
        {
            ratKing.Distract(time);
            action?.Invoke(rat.transform.parent.gameObject);
        }
    }
}
