public class PositionHolder : UnityEngine.MonoBehaviour
{
    public string id;
    
    void Reset() => 
        id = System.Guid.NewGuid().ToString();

    void Start()
    {
        var data = SceneData.Data;

        if (data.level != UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex || data.version != SaveData.currentVersion)
            return;

        var pos = data.positions.Find(i => i.id == id);

        if (pos != null)
            transform.position = pos.position;
    }
}
