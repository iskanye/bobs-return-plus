using static UnityEngine.SceneManagement.SceneManager;

public class SceneDataWarp : UnityEngine.MonoBehaviour
{
    public TMPro.TMP_Text savingText;

    void Awake() =>
        SceneData.Active.savingText = savingText;

    public void Save() =>
        SceneData.Save();

    public void DeleteSaves() =>
        SceneData.DeleteSaves();

    public void Reload() =>
        LoadScene(GetActiveScene().buildIndex);

}
