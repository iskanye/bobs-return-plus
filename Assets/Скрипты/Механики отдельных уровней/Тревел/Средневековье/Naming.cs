using UnityEngine;

public class Naming : MonoBehaviour
{
    public PlayerWarp warp;
    public GameObject namingWindow;
    public GameObject inventoryLayout;
    public TMPro.TMP_InputField inputField;
    public DialogueActivator confirmation;
    public UninteractiveDialogueActivator activator;
    public UninteractiveDialogue afterNaming;
    public UninteractiveDialogue afterNamingFinger;

    GameObject obj;

    public void StartNaming(GameObject obj) 
    {
        this.obj = obj;
        namingWindow.SetActive(true);
        inventoryLayout.SetActive(false);
        warp.player.Disable();
    }

    public void Confirm()
    {        
        namingWindow.SetActive(false);

        var dials = confirmation.dialogues;
        dials[0].text = $"Я правда хочу его назвать \"{inputField.text}\"?";
        DialogueSystem.StartDialogue(dials, obj);
    }

    public void Stop()
    {
        var dialogue = afterNaming; 

        if (inputField.text.ToLower() == "палец" || inputField.text.ToLower() == "finger" || inputField.text.ToLower() == "топор" || inputField.text.ToLower() == "пальчик")
            dialogue.dialogues = afterNamingFinger.dialogues; 
                 
        for (int i = 0; i < dialogue.dialogues.Length; i++)
            dialogue.dialogues[i].text = dialogue.dialogues[i].text.Replace("{топор}", inputField.text);

        activator.dialogues = dialogue;
        activator.Dialogue(obj);  

        for (int i = 0; i < dialogue.dialogues.Length; i++)
            dialogue.dialogues[i].text = dialogue.dialogues[i].text.Replace(inputField.text, "{топор}");       
        
        warp.Enabled = true;
        warp.player.Enable();
        
        inventoryLayout.SetActive(true);
    }
}
