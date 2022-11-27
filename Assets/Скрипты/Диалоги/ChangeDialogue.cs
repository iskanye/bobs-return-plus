using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class ChangeDialogue : MonoBehaviour
{
    public DialogueActivator interactiveActivator;
    public UninteractiveDialogueActivator uninteractiveActivator;
    public UninteractiveDialogue uninteractiveDialogue;
    public UnityEvent<GameObject>[] uninteractiveEvents;
    public Dialogue[] interactiveDialogue;
    
    void Awake() 
    {
        if (uninteractiveEvents.Length == 0) uninteractiveEvents = new UnityEvent<GameObject>[uninteractiveDialogue.dialogues.Length];
    }

    public void Change()
    {
        if (uninteractiveDialogue != null)
        {
            if (uninteractiveActivator != null) 
            {
                uninteractiveActivator.dialogues = uninteractiveDialogue;
                uninteractiveActivator.events = uninteractiveEvents;
                return;
            }

            List<Dialogue> dial = new List<Dialogue>();
            var cache = uninteractiveDialogue.dialogues;

            for (int i = 0; i < cache.Length; i++) 
                dial.Add(new Dialogue(cache[i], uninteractiveEvents[i]));

            interactiveActivator.dialogues = dial.ToArray();
            return;
        }

        interactiveActivator.dialogues = interactiveDialogue;
    }

    public void Change(bool prop) 
    {
        if (prop) 
            Change();
    }
}
