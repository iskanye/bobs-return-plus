using UnityEngine;

public class GlobalPropertyHolder : UnityEngine.MonoBehaviour
{
    public string id;
    public UnityEngine.Events.UnityEvent<bool> action;

    void Reset() => 
        id = System.Guid.NewGuid().ToString();

    public void SetProperty()
    {
        if (PlayerPrefs.HasKey(id))
            return;
        
        PlayerPrefs.SetInt(id, 0);
    }

    void Start()
    {
        if (PlayerPrefs.HasKey(id))
            action.Invoke(true);
    }
}
