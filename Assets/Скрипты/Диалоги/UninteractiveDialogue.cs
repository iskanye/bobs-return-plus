using UnityEngine;

[CreateAssetMenu(fileName = "New Uninteractive Dialogue", menuName = "Uninteractive Dialogue", order = 0)]
public class UninteractiveDialogue : ScriptableObject 
{
    [System.Serializable]
    public struct Dialog
    {
        public string character;
        public string text;
    }
    public Dialog[] dialogues;
}
