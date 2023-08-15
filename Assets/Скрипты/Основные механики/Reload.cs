using static UnityEngine.SceneManagement.SceneManager;

public class Reload : UnityEngine.MonoBehaviour
{
    public void ReloadLevel() =>
        LoadScene(GetActiveScene().buildIndex);
}
