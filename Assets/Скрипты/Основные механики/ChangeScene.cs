public class ChangeScene : UnityEngine.MonoBehaviour
{
    public int sceneIndex;

    public void Change() =>
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
}
