public class IntegerHolder : GUIDHolder
{
    public UnityEngine.MonoBehaviour integer;

    public IInteger Integer => integer as IInteger;

    void Start()
    {
        var data = SceneData.Data;

        if (data.level != UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex || data.version != SaveData.currentVersion)
            return;

        var prop = data.integers.Find(i => i.id == id);

        if (prop != null)
            Integer.integer = prop.integer;
    }
}
