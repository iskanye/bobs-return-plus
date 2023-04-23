using UnityEngine;

[ExecuteInEditMode]
public class FPSMeter : MonoBehaviour
{
    public TMPro.TMP_Text text;

    void Start() =>
        StartCoroutine(FPS());

    System.Collections.IEnumerator FPS()
    {
        while (true)
        {
            text.text = $"FPS: {(int)(Time.timeScale / Time.deltaTime)}";
            yield return new WaitForSeconds(.5f);
        }
    }
}
