using UnityEngine;

[CreateAssetMenu(fileName = "New Uninteractive Dialogue", menuName = "Uninteractive Dialogue", order = 0)]
public class UninteractiveDialogue : ScriptableObject 
{
    [System.Serializable]
    public class Dialog
    {
        public string character;
        public Sprite characterSprite;
        public string text;

        public float startDelay;

        public bool clearPreviousText = true;
        public bool showStraightaway;
        public bool dontWait;
    }
    public Dialog[] dialogues;
}
