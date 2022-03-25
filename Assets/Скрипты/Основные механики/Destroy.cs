using UnityEngine;

public class Destroy : MonoBehaviour 
{
    public GameObject objectToDestroy;

    public void Do() =>
        Destroy(objectToDestroy);

    public void Do(bool property)
    {
        if (property)
            Do();
    }

    public void Do(GameObject obj) =>
        Destroy(obj);
}
