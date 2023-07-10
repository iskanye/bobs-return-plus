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
    public TMPro.TMP_Text text;
    public GameObject textBox;

    IEnumerator Start()
    {
        foreach (var i in frames)
        {            
            textBox.SetActive(i.showTextBox);
            float t = 0, time = Time.time;
            spriteRenderer.sprite = i.sprite;

            while (t < 1 && i.fadeIn) 
            {
                spriteRenderer.color = Color.Lerp(Color.clear, Color.white, t);
                t += .01f;
                yield return new WaitForSeconds(.02425f); // .2425f
            }
            Debug.Log(Time.time - time);
            
            spriteRenderer.color = Color.white;

            if (i.text != "")
            {
                text.text = i.text;            
                text.ForceMeshUpdate();
                yield return TextUtilities.MakeTextTransparent(text);
                StartCoroutine(TextUtilities.AnimateVertexColors(text, Color.white, delay: .05f));
            }

            yield return new WaitForSeconds(i.waitTime);
            
            while (t > 0 && i.fadeIn) 
            {
                spriteRenderer.color = Color.Lerp(Color.clear, Color.white, t);
                t -= .01f;
                yield return new WaitForSeconds(.02425f);
            }
            
            text.text = "";
        }
    }
}
