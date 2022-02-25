public abstract class SequenceObject : UnityEngine.MonoBehaviour
{
    public virtual System.Collections.IEnumerator Sequence() 
    {
        yield break; 
    }
}
