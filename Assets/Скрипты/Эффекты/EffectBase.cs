using System.Collections;
using UnityEngine;

public abstract class EffectBase : ScriptableObject, System.IEquatable<EffectBase>
{
    [HideInInspector] public EffectsController mn;
    public float duration;

    public IEnumerator start;
    public IEnumerator process;

    protected EffectData data;

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
        mn.StopCoroutine(start);
        mn.effects.Remove(this);
        yield break;
    }

    public bool Equals(EffectBase e)
    {
        if (e == null)
            return false;

        if (process == e.Process())
            return true;

        return false;
    }
}
