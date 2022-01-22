using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class ChangeDialogue : MonoBehaviour
{
    public DialogueActivator interactiveActivator;
    public UninteractiveDialogueActivator uninteractiveActivator;
    public UninteractiveDialogue uninteractiveDialogue;
    public UnityEvent[] uninteractiveEvents;
    public Dialogue[] interactiveDialogue;
    
    void Awake() 
    {
        if (uninteractiveEvents.Length == 0) uninteractiveEvents = new UnityEvent[uninteractiveDialogue.dialogues.Length];
    }

    public void Change()
    {
        if (uninteractiveDialogue != null)
        {
            if (uninteractiveActivator != null) 
            {
                uninteractiveActivator.dialogues = uninteractiveDialogue;
                return;
            }
            
            List<Dialogue> dial = new List<Dialogue>();
            var cache = uninteractiveDialogue.dialogues;
            for (int i = 0; i < cache.Length; i++) dial.Add(new Dialogue(cache[i].character, cache[i].text, uninteractiveEvents[i]));
            interactiveActivator.dialogues = dial.ToArray();
            return;
        }

        interactiveActivator.dialogues = interactiveDialogue;
    }

    public void Change(bool prop) 
    {
        if (prop) Change();
    }
}
