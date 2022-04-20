using UnityEngine;

public class PlayerEffectsController : EffectsController
{
    public UnityEngine.UI.Image[] images;

    public static PlayerEffectsController Active { get; private set; }

    void Awake() =>
        Active = this;

    void Update() 
    {
        if (images != null)
            for (int i = 0; i < images.Length; i++)
            {
                if (i < effects.Count)
                {
                    images[i].sprite = (effects[i] as PlayerEffectBase).sprite;
                    images[i].color = Color.white;
                }

                else
                    images[i].color = new Color(0, 0, 0, 0);
            }
    }
}
