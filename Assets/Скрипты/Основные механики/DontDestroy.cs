using UnityEngine;

public class DontDestroy : MonoBehaviour 
{
    static DontDestroy Active;

    void Awake()
    {
        if (Active == null)
        {
            Active = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
            return;
        }
    }
}