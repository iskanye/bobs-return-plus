using UnityEngine;

public class CreateGameObject : MonoBehaviour
{
    public GameObject[] _objects;
    public System.Action<GameObject> onCreate;

    public void Create() 
    {
        if (onCreate == null)
            foreach (var i in _objects)
                Instantiate(i, transform.position, Quaternion.identity);

        else 
            foreach (var i in _objects)
                onCreate.Invoke(Instantiate(i, transform.position, Quaternion.identity));
    }
}
