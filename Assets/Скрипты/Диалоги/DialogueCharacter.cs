using UnityEngine;

[CreateAssetMenu(fileName = "DialogueCharacter", menuName = "Dialogue Character", order = 0)]
public class DialogueCharacter : ScriptableObject
{
    public System.Collections.Generic.Dictionary<int, Sprite> emotions;
}
