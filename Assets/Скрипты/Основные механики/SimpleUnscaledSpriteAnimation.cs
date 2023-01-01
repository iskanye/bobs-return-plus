using UnityEngine;

public class SimpleUnscaledSpriteAnimation : MonoBehaviour
{
    public new SpriteRenderer renderer;
    public UnityEngine.UI.Image image;
    public Sprite[] sprites;
    public float delay;
    public bool isAwake = true;

    int index;

    void Start() 
    {
        if (isAwake)
            StartAnimation();
    }

    System.Collections.IEnumerator Animation() 
    {
        if (renderer != null)
            renderer.sprite = sprites[index];

        else
            image.sprite = sprites[index];

        yield return new WaitForSecondsRealtime(delay);

        index++;
        index %= sprites.Length;

        StartCoroutine(Animation());
    }

    public void Stop() =>
        StopAllCoroutines();

    public void StartAnimation() =>
        StartCoroutine(Animation());
}
