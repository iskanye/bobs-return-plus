using UnityEngine;

public class Disable : MonoBehaviour
{
    public void Do(GameObject o) =>
        o.SetActive(false);
}
