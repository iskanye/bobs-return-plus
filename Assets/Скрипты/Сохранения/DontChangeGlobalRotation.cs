using UnityEngine;

[ExecuteInEditMode]
public class DontChangeGlobalRotation : MonoBehaviour
{
    void Update() =>
        transform.rotation = Quaternion.identity;
}
