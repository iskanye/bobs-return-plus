using UnityEngine;
using UnityEngine.Events;

public class UninteractiveDialogueActivator : MonoBehaviour
{
    public UninteractiveDialogue dialogues;
    public UnityEvent<GameObject>[] events;
    
    public void Dialogue(GameObject obj) 
    {
        if (dialogues == null)
            return;

        var dials = dialogues.dialogues;
        var convert = new Dialogue[dials.Length];

        for (int i = 0; i < dials.Length; i++) 
            convert[i] = new Dialogue(dials[i], events.Length <= 0 ? null : events[i]);

        DialogueSystem.StartDialogue(convert, obj);
    }

    public void Dialogue() =>
        Dialogue(null);
}
