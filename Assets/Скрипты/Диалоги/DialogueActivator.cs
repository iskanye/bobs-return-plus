public class DialogueActivator : UnityEngine.MonoBehaviour
{
    public DialogueSystem system;
    public Dialogue[] dialogues;
    
    public void Dialogue() => system.StartDialogue(dialogues);
}
