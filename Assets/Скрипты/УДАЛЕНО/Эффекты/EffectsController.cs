using UnityEngine;
using System.Collections.Generic;

public class EffectsController : MonoBehaviour
{
    public EffectData data;
    public List<EffectBase> effects;

    public void AddEffect(EffectBase effect) 
    {
        if (effects.Contains(effect))
            return;

        effect.mn = this;
        effect.data = data;
        effects.Add(effect);

        effect.start = effect.Start();
        StartCoroutine(effect.start);
    }

    public void StopEffect(EffectBase effect) 
    {
        for (int i = 0; i < effects.Count; i++)
            if (effects[i].Equals(effect)) 
                StartCoroutine(effects[i].Stop());
    }
}
[System.Serializable]
public struct EffectData
{
    public BaseMovement movement;
    public LivesBase lives;
    public Damageable damageable;
    public GameObject gameObject;
}
