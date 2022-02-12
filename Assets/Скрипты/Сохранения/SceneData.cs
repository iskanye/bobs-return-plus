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
    LiveCounter lives;

    void Awake()
    {
        Active = this;
        positions = FindObjectsOfType<PositionHolder>();
        properties = FindObjectsOfType<PropertyHolder>();
        globalProperties = FindObjectsOfType<GlobalPropertyHolder>();
        lives = FindObjectOfType<LiveCounter>();
        Data = SaveLoad.Load();

        if (Data.level != GetActiveScene().buildIndex) 
        {
            Data = new SaveData();
            return;
        }

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

    public void Save()
    {
        Data.customProperties = new List<CustomProperty>();
        Data.level = GetActiveScene().buildIndex;
        Data.lives = lives.livesRemaining;

        foreach (var i in positions) Data.positions.Add(new Position(i.id, i.gameObject.transform.position));

        foreach (var i in properties) Data.customProperties.Add(new CustomProperty(i.id, i.property));

        StopAllCoroutines();
        StartCoroutine(Saving());
    }

    public void DeleteSaves()
    {
        StopAllCoroutines();
        StartCoroutine(Deleting());
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
        ResetData();

        savingText.text = "удалено";
        yield return new WaitForSecondsRealtime(3);
        savingText.text = "";
    }

    void ResetData() 
    {
        Data.customProperties = new List<CustomProperty>();
        Data.globalProperties = new List<CustomProperty>();
        Data.positions = new List<Position>();
        Data.achievements = new List<IdItem>();
        Data.inventory = new List<IdItem>();
    }
}
