public class DialogueActivator : UnityEngine.MonoBehaviour
{
    public Dialogue[] dialogues;

    public void Dialogue() => DialogueSystem.StartDialogue(dialogues);
}
