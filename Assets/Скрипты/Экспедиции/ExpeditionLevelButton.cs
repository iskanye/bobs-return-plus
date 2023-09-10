using UnityEngine;
using UnityEngine.EventSystems;

public class ExpeditionLevelButton : MonoBehaviour, IPointerClickHandler
{
    public ExpeditionLevel level;
    public UnityEngine.UI.Image image;
    public ExpeditionController expeditionController;

    public void OnPointerClick(PointerEventData e) =>
        expeditionController.ChooseLevel(level);
}
