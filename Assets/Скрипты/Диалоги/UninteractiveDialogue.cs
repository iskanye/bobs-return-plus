using UnityEngine;

[CreateAssetMenu(fileName = "New Uninteractive Dialogue", menuName = "Uninteractive Dialogue", order = 0)]
public class UninteractiveDialogue : ScriptableObject 
{
    [System.Serializable]
    public class Dialog
    {
        public string character;
        public DialogueCharacter dialogueCharacter;
        public DialogueCharacter.Emotion emotion;
        public Color color = Color.white;
        public float delay = .015f;
        [TextArea] public string text;

        public float startDelay;

        public bool clearPreviousText = true;
        public bool showStraightaway;
        public bool dontWait;
        public bool cantSkip;
    }
    public Dialog[] dialogues;
}
