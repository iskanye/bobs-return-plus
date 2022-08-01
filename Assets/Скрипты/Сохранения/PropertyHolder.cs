public class PropertyHolder : UnityEngine.MonoBehaviour
{
    public string id;
    public bool property { set; get; }
    public UnityEngine.Events.UnityEvent<bool> action;

    void Reset() => 
        id = System.Guid.NewGuid().ToString();

    void Start() 
    {
        var data = SceneData.Data;

        if (data.level != UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex || data.version != SaveData.currentVersion)
            return;

        var prop = data.customProperties.Find(i => i.id == id);

        if (prop != null) 
        {
            property = prop.property;
            action.Invoke(property); 
        }
    }
}
