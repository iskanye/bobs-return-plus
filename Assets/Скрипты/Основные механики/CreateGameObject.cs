using UnityEngine;

public class CreateGameObject : MonoBehaviour
{
    public GameObject _object;

    public void Create() =>
        Instantiate(_object, transform.position, Quaternion.identity);
}
