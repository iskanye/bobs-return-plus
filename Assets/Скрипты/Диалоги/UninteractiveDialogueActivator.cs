using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class UninteractiveDialogueActivator : MonoBehaviour
{
    public DialogueSystem system;
    public UninteractiveDialogue dialogues;
    public UnityEvent[] events;
    
    void Awake() 
    {
        if (events == null) events = new UnityEvent[dialogues.dialogues.Length];
    }

    public void Dialogue() 
    {
        List<Dialogue> dial = new List<Dialogue>();
        var cache = dialogues.dialogues;
        for (int i = 0; i < cache.Length; i++) dial.Add(new Dialogue(cache[i].character, cache[i].text, events[i]));
        system.StartDialogue(dial.ToArray());
    }
}
