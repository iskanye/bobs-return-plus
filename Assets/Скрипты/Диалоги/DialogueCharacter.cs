using UnityEngine;

[CreateAssetMenu(fileName = "DialogueCharacter", menuName = "Dialogue Character", order = 0)]
public class DialogueCharacter : ScriptableObject
{
    public enum Emotion
    {
        Idle,
        Happy,
        Fear,
        Angry,
        Sad,
        Schizophrenic,
        Other
    }
    public System.Collections.Generic.Dictionary<Emotion, Sprite> emotions;
}
