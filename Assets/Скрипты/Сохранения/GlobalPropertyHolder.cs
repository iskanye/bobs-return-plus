using UnityEngine;

public class GlobalPropertyHolder : GUIDHolder
{
    public UnityEngine.Events.UnityEvent<bool> action;

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
