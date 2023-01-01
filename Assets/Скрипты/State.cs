using System.Collections;

public abstract class State<T> where T : UnityEngine.MonoBehaviour
{
    protected T mn;

    protected State(T manager) => 
        mn = manager;

    public virtual IEnumerator Start()
    {
        mn.StartCoroutine(Update());
        yield break;
    }

    //Считать как за один проход по циклу. Если нужен сам цикл - использовать while(true)
    public virtual IEnumerator Update()
    {
        yield return null;
    }

    public virtual IEnumerator Stop()
    {
        mn.StopAllCoroutines();
        yield break;
    }
}
