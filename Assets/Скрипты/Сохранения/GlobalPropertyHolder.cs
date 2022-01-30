using UnityEngine;

public class GlobalPropertyHolder : MonoBehaviour
{
    public string id;
    public UnityEngine.Events.UnityEvent<bool> action;

    public SceneData data { set; private get; }

    void Reset() => id = System.Guid.NewGuid().ToString();

    public void SetProperty(bool property)
    {
        var cache = data.data.globalProperties;       
        var prop = cache.Find(p => p.id == id);

        if (prop != null)
            data.data.globalProperties.Find(p => p.id == id).property = property;

        else 
            data.data.globalProperties.Add(new CustomProperty(id, property));
    }
}
