public class GlobalObject : UnityEngine.MonoBehaviour
{
    public static GlobalObject Active;

    void Awake() 
    {
        if (Active == null) 
        {
            Active = this;
            DontDestroyOnLoad(gameObject);
        }

        else
            Destroy(gameObject);
    }
}
