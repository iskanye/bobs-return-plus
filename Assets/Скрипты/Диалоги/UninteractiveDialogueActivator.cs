using UnityEngine;
using UnityEngine.Events;

public class UninteractiveDialogueActivator : MonoBehaviour
{
    public UninteractiveDialogue dialogues;
    public UnityEvent[] events;
    
    void Awake() 
    {
        if (events.Length <= 0) events = new UnityEvent[dialogues.dialogues.Length];
    }

    public void Dialogue() 
    {
        var dials = dialogues.dialogues;
        var convert = new Dialogue[dials.Length];

        for (int i = 0; i < dials.Length; i++) convert[i] = new Dialogue(dials[i].character, dials[i].text, events[i]);

        DialogueSystem.Active.StartDialogue(convert);
    }
}
