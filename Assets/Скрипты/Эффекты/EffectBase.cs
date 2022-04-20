using System.Collections;
using UnityEngine;

public abstract class EffectBase : ScriptableObject
{
    [HideInInspector] public EffectsController mn;
    public float duration;

    protected EffectData data;

    IEnumerator process;

    public virtual IEnumerator Start()
    {
        data = mn.data;
        process = Process();
        mn.StartCoroutine(process);

        yield return new WaitForSeconds(duration);
        mn.StartCoroutine(Stop());
    }

    public virtual IEnumerator Process()
    {
        yield break;
    }

    public virtual IEnumerator Stop()
    {
        mn.StopCoroutine(process);
        mn.effects.Remove(this);
        yield break;
    }
}
