using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ContainerController controller;
    public int index;    
    public Image icon;
    public Image dragPrefab;

    public Item item 
    {
        set 
        {
            if (value == null)
                icon.color = new Color(0, 0, 0, 0);
            
            else 
            {
                icon.sprite = value.icon;
                icon.color = Color.white;
            }
            controller.onItemChange?.Invoke(value, this);
            _item = value;
        }
        get =>
            _item;
    }

    Image drag;
    [SerializeField] Item _item;

    void Start() =>
        item = _item;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_item == null)
            return;

        drag = Instantiate(dragPrefab, AlwaysOnTopCanvas.Active.transform);
        drag.sprite = _item.icon;

        drag.rectTransform.anchoredPosition = (eventData.position - new Vector2(Display.main.systemWidth, Display.main.systemHeight) / 2) 
            / Constants.scaleFactor;  
            //вычитаем половину позиции экрана так как для экрана точка (0, 0) это слева снизу а для юнити точка (0, 0) посередине экрана
            //еще делим на степень масштабирование чтобы позиция не слетела

        icon.color = new Color(0, 0, 0, 0);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_item == null)
            return;

        drag.rectTransform.anchoredPosition += eventData.delta / Constants.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_item == null)
            return;
        
        List<RaycastResult> result = new();
        EventSystem.current.RaycastAll(eventData, result);

        bool foundButton = false;

        foreach (var i in result) 
            if (i.gameObject.TryGetComponent(out ItemButton button) && button != this)
            {
                button.item = _item;
                item = null;
                foundButton = true;
                break;
            }

        if (!foundButton)
            icon.color = Color.white;

        Destroy(drag.gameObject);
        drag = null;
    }
}
