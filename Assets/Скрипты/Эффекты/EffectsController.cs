using UnityEngine;
using System.Collections.Generic;

public class EffectsController : MonoBehaviour
{
    public EffectData data;
    [HideInInspector] public List<EffectBase> effects;

    public void AddEffect(EffectBase effect) 
    {
        if (effects.Contains(effect))
            return;

        effect.mn = this;
        effects.Add(effect);
        StartCoroutine(effect.Start());
    }
}

[System.Serializable] 
public class EffectData 
{
    public BaseMovement movement;
    public LivesBase lives;
    public Damageable damageable;
    public GameObject gameObject;
}