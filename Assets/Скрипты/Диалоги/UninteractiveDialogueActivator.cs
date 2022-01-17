using UnityEngine;
using System.Collections.Generic;

public class UninteractiveDialogueActivator : MonoBehaviour
{
    public DialogueSystem system;
    public UninteractiveDialogue dialogues;
    
    public void Dialogue() 
    {
        List<Dialogue> dial = new List<Dialogue>();
        foreach (var i in dialogues.dialogues) dial.Add(new Dialogue(i.character, i.text, i.action));
        system.StartDialogue(dial.ToArray());
    }
}
