using UnityEngine.EventSystems;

public class DialogueVariantObject : UnityEngine.MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public int index;
    public DialogueSystem system;

    public void OnPointerClick(PointerEventData eventData) =>
        system.ChooseVariant();

    public void OnPointerEnter(PointerEventData eventData) =>
        system.ChangeVariant(index);
}
