using UnityEngine;

[CreateAssetMenu(fileName = "DialogueCharacter", menuName = "Dialogue Character", order = 0)]
public class DialogueCharacter : ScriptableObject
{
    public System.Collections.Generic.List<Emotion> emotions;

    [System.Serializable]
    public struct Emotion
    {
        public int id;
        public Sprite sprite;
    }
}
