using System.Collections;
using UnityEngine;
using Dialogues;

public class PrologueCutscene : MonoBehaviour
{
    [System.Serializable]
    public struct Frame
    {
        public Sprite sprite;
        public string text;
        public float waitTime;
        public bool fadeIn;
        public bool showTextBox;
    }

    public Frame[] frames;
    public SpriteRenderer spriteRenderer;
    public Animator spriteFade;
    public TMPro.TMP_Text text;
    public GameObject textBox;
    public float finalWaitTime;
    public int nextSceneIndex;

    IEnumerator Start()
    {
        foreach (var i in frames)
        {            
            textBox.SetActive(i.showTextBox);
            spriteRenderer.sprite = i.sprite;

            if (i.fadeIn)
            {
                spriteFade.Play("Appear");
                yield return new WaitForSecondsRealtime(2.5f);
            }
            
            else            
                spriteFade.Play("Idle1");               

            if (i.text != "")
            {
                text.text = i.text;            
                text.ForceMeshUpdate();
                yield return TextUtilities.MakeTextTransparent(text);
                StartCoroutine(TextUtilities.AnimateVertexColors(text, Color.white, delay: .05f));
            }

            yield return new WaitForSecondsRealtime(i.waitTime);

            if (i.fadeIn)
            {
                spriteFade.Play("Disappear");
                yield return new WaitForSecondsRealtime(2.5f);
            }

            text.text = "";
        }

        yield return new WaitForSecondsRealtime(finalWaitTime);
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneIndex);
    }
}
