using UnityEngine;

public class AlwaysOnTopCanvas : MonoBehaviour
{
    public static AlwaysOnTopCanvas Active { private set; get; }

    void Awake() =>
        Active = this;

    public static void PutOnTop(Transform obj) =>
        obj.SetParent(Active.transform);
}
