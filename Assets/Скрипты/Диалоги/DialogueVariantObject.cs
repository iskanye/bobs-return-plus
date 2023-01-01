using UnityEngine.EventSystems;

public class DialogueVariantObject : UnityEngine.MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    public int index;
    public DialogueSystem system;

    public void OnPointerDown(PointerEventData eventData) =>
        system.ChooseVariant();

    public void OnPointerEnter(PointerEventData eventData) =>
        system.ChangeVariant(index);
}
