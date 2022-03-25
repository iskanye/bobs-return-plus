using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using static UnityEngine.SceneManagement.SceneManager;

public class SceneData : MonoBehaviour
{
    public TMPro.TMP_Text savingText;

    public static SaveData Data { get; private set; }

    public static SceneData Active { get; private set; }

    PositionHolder[] positions;
    PropertyHolder[] properties;
    GlobalPropertyHolder[] globalProperties;
    PlayerLiveCounter lives;
    InventorySystem inventory;

    void Awake()
    {
        if (Active == null)
        {
            Active = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
            return;
        }

        Data = SaveLoad.Load();
    }

    void Start()
    {
        positions = FindObjectsOfType<PositionHolder>();
        properties = FindObjectsOfType<PropertyHolder>();
        globalProperties = FindObjectsOfType<GlobalPropertyHolder>();
        inventory = FindObjectOfType<InventorySystem>();
        lives = PlayerLiveCounter.Active;

        if (Data.level != GetActiveScene().buildIndex)
            return;

        foreach (var pos in Data.positions)
        {
            var pr = positions.First(i => i.id == pos.id);

            if (pr == null)
                continue;

            pr.gameObject.transform.position = pos.position;
        }

        foreach (var prop in Data.customProperties)
        {
            var pr = properties.First(i => i.id == prop.id);

            if (pr == null)
                continue;

            pr.property = prop.property;
            pr.action.Invoke(pr.property);
        }

        foreach (var pr in globalProperties)
            foreach (var prop in Data.globalProperties)
                if (pr.id == prop.id)
                    pr.action.Invoke(prop.property);
    }

    public static void Save()
    {
        var active = Active;

        Data.customProperties = new List<CustomProperty>();
        Data.inventory = new List<GuidItem>();
        Data.level = GetActiveScene().buildIndex;
        Data.lives = (int)active.lives.LivesRemaining;

        foreach (var i in active.positions) 
            Data.positions.Add(new Position(i.id, i.gameObject.transform.position));

        foreach (var i in active.properties) 
            Data.customProperties.Add(new CustomProperty(i.id, i.property));

        foreach (var i in active.inventory.items)
            if (i != null) 
                Data.inventory.Add(new GuidItem(i.id));

        active.StopAllCoroutines();
        active.StartCoroutine(active.Saving());
    }

    public static void DeleteSaves()
    {
        var active = Active;

        active.StopAllCoroutines();
        active.StartCoroutine(active.Deleting());
    }

    IEnumerator Saving()
    {
        savingText.text = "сохраняется";

        SaveLoad.Save(Data);

        savingText.text = "сохранено";
        yield return new WaitForSecondsRealtime(3);
        savingText.text = "";
    }

    IEnumerator Deleting()
    {
        savingText.text = "удаляется";

        SaveLoad.DeleteSaves();
        Data = new SaveData();

        savingText.text = "удалено";
        yield return new WaitForSecondsRealtime(3);
        savingText.text = "";
    }
}
