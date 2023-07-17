using UnityEngine;

public class Destroy : MonoBehaviour 
{
    public Object[] objectsToDestroy;

    public void Do() 
    {
        foreach (var i in objectsToDestroy)
            Destroy(i);
    }
    public void Do(bool property)
    {
        if (property)
            Do();
    }

    public void Do(Object obj) =>
        Destroy(obj);

}
