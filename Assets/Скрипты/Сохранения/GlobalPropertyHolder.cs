using UnityEngine;

public class GlobalPropertyHolder : MonoBehaviour
{
    public string id;
    public UnityEngine.Events.UnityEvent action;

    public void SetProperty()
    {
        if (PlayerPrefs.HasKey(id))
            return;
        
        PlayerPrefs.SetInt(id, 0);
    }

    void Start()
    {
        if (PlayerPrefs.HasKey(id))
            action.Invoke();
    }
}
