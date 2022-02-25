public class GlobalPropertyHolder : UnityEngine.MonoBehaviour
{
    public string id;
    public UnityEngine.Events.UnityEvent<bool> action;

    void Reset() => id = System.Guid.NewGuid().ToString();

    public void SetProperty(bool property)
    {
        ref var cache = ref SceneData.Data.globalProperties;       
        var prop = cache.Find(p => p.id == id);

        if (prop != null)
            cache.Find(p => p.id == id).property = property;

        else
            cache.Add(new CustomProperty(id, property));
    }
}
