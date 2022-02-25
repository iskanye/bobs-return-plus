using UnityEngine;

public class Destroy : MonoBehaviour 
{
    public GameObject objectToDestroy;

    public void Do() =>
        Destroy(objectToDestroy);
}
