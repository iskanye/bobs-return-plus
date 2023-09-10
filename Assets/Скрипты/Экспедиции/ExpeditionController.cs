using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ExpeditionController : MonoBehaviour
{
    [Header("Levels")]
    public GameObject levelsLayout;
    public Transform mapView;
    public ExpeditionLevel[] levels;
    public ExpeditionLevelButton levelButtonPrefab;
    public RectTransform connectionPrefab;
    public Transform listOfLevels;
    public TMP_Text episodeTitle;
    public TMP_Text levelTitle;
    public TMP_Text levelDescription;
    public Image levelImage;

    [Header("Items")]
    public GameObject itemsLayout;
    public Item[] items;
    public ExpeditionItem[] inventoryItems;
    public Image[] itemsImage;

    [Header("Characters")]
    public GameObject charactersLayout;

    [Header("Other")]
    public RectTransform selection;

    GameObject selectedObj;

    List<RectTransform> mapConnections = new();
    ExpeditionLevel selectedLevel;

    int selectedItemId;
    Item[] selectedItems = new Item[6];

    int selectedBobId;

    void Start()
    {
        foreach (var i in levels)
        {
            var button = Instantiate(levelButtonPrefab, listOfLevels);
            button.level = i;
            button.image.sprite = i.icon;
            button.expeditionController = this;
        }
    } 

    // Функция для отрисовки карты
    void DrawLevel(ExpeditionLevel level)
    {
        mapConnections.ForEach(i => Destroy(i.gameObject));
        mapConnections.Clear();
        List<LevelPoint> usedPoints = new();

        //Функция для отрисовки соединений между точками карты
        void DrawPointConnections(LevelPoint p)
        {
            usedPoints.Add(p);

            foreach (var i in p.connectedPoints) 
                if (!usedPoints.Contains(i))
                {
                    var connection = Instantiate(connectionPrefab, mapView);
                    mapConnections.Add(connection);
                    connection.anchoredPosition = p.position;
                    connection.sizeDelta = new Vector2((i.position - p.position).magnitude, connection.sizeDelta.y);
                    //Высчитываю угол линии через арктангенс
                    connection.Rotate(0, 0, (float)Mathf.Atan2(i.position.y - p.position.y, i.position.x - p.position.x) * Mathf.Rad2Deg);
                    //Повторяю весь процесс для точек, с которыми соединена данная точка, кроме тех, по которым функция уже прошлась
                    DrawPointConnections(i);
                }
        }

        DrawPointConnections(level.rootPoint);
    }  

    void Update()
    {
        levelImage.color = selectedLevel == null ? Color.clear : Color.white;
        levelImage.sprite = selectedLevel?.icon;

        for (int i = 0; i < 6; i++)
        {
            itemsImage[i].color = Color.clear;
        }
    }

    //Функции по открытию меню выбора уровня/предмета/Боба

    public void SelectLevel()
    {
        levelsLayout.SetActive(true);
        itemsLayout.SetActive(false);
        charactersLayout.SetActive(false);
    }

    public void SelectItem(int id)
    {
        levelsLayout.SetActive(false);
        itemsLayout.SetActive(true);
        charactersLayout.SetActive(false);
        selectedItemId = id;
    }

    public void SelectBob(int id)
    {
        levelsLayout.SetActive(false);
        itemsLayout.SetActive(false);
        charactersLayout.SetActive(true);
        selectedBobId = id;
    }

    //Функции по выбору уровня/предмета/Боба

    public void ChooseLevel(ExpeditionLevel level)
    {
        selectedLevel = level;
        episodeTitle.text = level.episodeTitle;
        levelTitle.text = level.title;
        levelDescription.text = level.briefDescription;
        DrawLevel(level);
    }

    public void ChooseItem(Item item)
    {
        selectedItems[selectedItemId] = item;
    }

    public void ChooseBob() 
    {

    }

    public void Select(RectTransform rect) 
    {
        selection.sizeDelta = rect.sizeDelta;
        selection.position = rect.position;

        selectedObj = rect.gameObject;
    }
}
