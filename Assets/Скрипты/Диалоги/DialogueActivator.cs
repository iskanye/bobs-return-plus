using UnityEngine;

public class DialogueActivator : MonoBehaviour
{
    public Dialogue[] dialogues;

    public void Dialogue() => 
        DialogueSystem.StartDialogue(dialogues, null);

    public void Dialogue(GameObject obj) =>
        DialogueSystem.StartDialogue(dialogues, obj);
}
