using UnityEngine;

public class Destroy : MonoBehaviour 
{
    public Object objectToDestroy;

    public void Do() =>
        Destroy(objectToDestroy);

    public void Do(bool property)
    {
        if (property)
            Do();
    }

    public void Do(Object obj) =>
        Destroy(obj);
}
