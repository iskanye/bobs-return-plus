using UnityEngine;

public class IntegerHolder : MonoBehaviour
{
    public string id;
    public MonoBehaviour integer;

    public IInteger Integer => integer as IInteger;

    void Reset() =>
        id = System.Guid.NewGuid().ToString();

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
