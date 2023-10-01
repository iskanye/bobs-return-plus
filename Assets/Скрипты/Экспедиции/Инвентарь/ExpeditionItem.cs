using UnityEngine;
using UnityEngine.EventSystems;

public class ExpeditionItem : MonoBehaviour, IPointerClickHandler
{
    public Item item;
    public UnityEngine.UI.Image image;
    public ExpeditionController expeditionController;

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
