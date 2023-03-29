using UnityEngine;
using System.Linq;

[RequireComponent(typeof(UninteractiveDialogueActivator))]
public class Container : ActionBase
{
    public Item[] items;
    [Range(0, 1)] 
    public float noItemPropability;
    public UninteractiveDialogue empty;

    public bool containsItems { get; set; } = true;

    UninteractiveDialogueActivator uninteractiveActivator;

    void Awake() =>
        uninteractiveActivator = GetComponent<UninteractiveDialogueActivator>();

    public void OpenContainer(GameObject obj) 
    {
        if (!containsItems || Random.value <= noItemPropability)
        {
            containsItems = false;
            
            uninteractiveActivator.dialogues = empty;
            uninteractiveActivator.Dialogue();
            return;
        }

        if ((items.Length == 1 && InventorySystem.Active.AddItem(items[0])) ||
            (items.Length >= 1 && InventorySystem.Active.AddItem(items[Random.Range(0, items.Length)])))
            containsItems = false;

        if (action != null)
            action.Invoke(obj);        
    }
}
