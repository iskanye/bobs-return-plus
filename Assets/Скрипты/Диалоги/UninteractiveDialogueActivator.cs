using UnityEngine;
using UnityEngine.Events;

public class UninteractiveDialogueActivator : MonoBehaviour
{
    public UninteractiveDialogue dialogues;
    public UnityEvent<GameObject>[] events;
    
    void Awake() 
    {
        if (events.Length <= 0) events = new UnityEvent<GameObject>[dialogues.dialogues.Length];
    }

    public void Dialogue(GameObject obj) 
    {
        if (dialogues == null)
            return;

        var dials = dialogues.dialogues;
        var convert = new Dialogue[dials.Length];

        for (int i = 0; i < dials.Length; i++) 
            convert[i] = new Dialogue(dials[i], events[i]);

        DialogueSystem.StartDialogue(convert, obj);
    }

    public void Dialogue() =>
        Dialogue(null);
}
