using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

public class ExpeditionController : MonoBehaviour
{
    [Header("Levels")]
    public Transform mapView;
    public ExpeditionLevel selectedLevel;
    public RectTransform connectionPrefab;

    [Header("Items")]
    public Item[] items;
    public ExpeditionItem itemButtonPrefab;
    public RectTransform itemsLayout;

    [Header("Characters")]

    [Header("Other")]
    public RectTransform selection;

    GameObject selectedObj;

    void Start()
    {
        List<LevelPoint> usedPoints = new();

        //Функция для отрисовки карты
        void DrawPointConnections(LevelPoint p)
        {
            usedPoints.Add(p);

            foreach (var i in p.connectedPoints) 
                if (!usedPoints.Contains(i))
                {
                    var connection = Instantiate(connectionPrefab, mapView);
                    connection.anchoredPosition = p.position;
                    connection.sizeDelta = new Vector2((i.position - p.position).magnitude, connection.sizeDelta.y);
                    //Высчитываю угол линии через арктангенс
                    connection.Rotate(0, 0, (float)Math.Atan2(i.position.y - p.position.y, i.position.x - p.position.x) * Mathf.Rad2Deg);
                    //Повторяю весь процесс для точек, с которыми соединена данная точка, кроме тех, по которым функция уже прошлась
                    DrawPointConnections(i);
                }
        }

        DrawPointConnections(selectedLevel.rootPoint);
    }   

    public void SelectLevel()
    {
        
    }

    public void SelectItem()
    {

    }

    public void SelectBob()
    {

    }

    public void Select(BaseEventData e) 
    {
        var rect = e.selectedObject.GetComponent<RectTransform>();

        selection.sizeDelta = rect.sizeDelta;
        selection.position = rect.position;

        selectedObj = e.selectedObject;
    }
}
