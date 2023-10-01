using System.Collections;
using UnityEngine;

public abstract class EffectBase : ScriptableObject, System.IEquatable<EffectBase>
{
    [HideInInspector] public EffectsController mn;
    [HideInInspector] public EffectData data;

    public string id;
    public float duration;

    public IEnumerator start;
    public IEnumerator process;

    public virtual IEnumerator Start()
    {
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

        if (id == e.id)
            return true;

        return false;
    }
}
