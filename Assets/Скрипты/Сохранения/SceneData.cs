using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using static UnityEngine.SceneManagement.SceneManager;

public class SceneData : MonoBehaviour
{
    public TMPro.TMP_Text savingText;

    public SaveData data { get; private set; }

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
        data = SaveLoad.Load();

        if (data.level != GetActiveScene().buildIndex) 
        {
            return;
        }

        foreach (var pos in data.positions)
        {
            var pr = positions.First(i => i.id == pos.id);

            if (pr == null)
                continue;

            pr.gameObject.transform.position = pos.position;
        }

        foreach (var prop in data.customProperties)
        {
            var pr = properties.First(i => i.id == prop.id);

            if (pr == null)
                continue;

            pr.property = prop.property;
            pr.action.Invoke(pr.property);
        }

        foreach (var pr in globalProperties)
        {
            pr.data = this;

            foreach (var prop in data.globalProperties)
                if (pr.id == prop.id)
                    pr.action.Invoke(prop.property);
        }

        lives.livesRemaining = data.lives;

        for (int i = 0; i < lives.lives.Length; i++) lives.lives[i].gameObject.SetActive(i <= data.lives - 1);
    }

    public void Save()
    {
        data.customProperties = new List<CustomProperty>();
        data.level = GetActiveScene().buildIndex;
        data.lives = lives.livesRemaining;

        foreach (var i in positions) data.positions.Add(new Position(i.id, i.gameObject.transform.position));

        foreach (var i in properties) data.customProperties.Add(new CustomProperty(i.id, i.property));

        SaveLoad.Save(data);
        StartCoroutine(Saved());
    }

    public void DeleteSaves()
    {
        SaveLoad.DeleteSaves();
        ResetData();
        StartCoroutine(Deleted());
    }

    IEnumerator Saved()
    {
        savingText.text = "сохранено";
        yield return new WaitForSecondsRealtime(3);
        savingText.text = "";
    }

    IEnumerator Deleted()
    {
        savingText.text = "удалено";
        yield return new WaitForSecondsRealtime(3);
        savingText.text = "";
    }
    void ResetData() 
    {
        data.customProperties = new List<CustomProperty>();
        data.globalProperties = new List<CustomProperty>();
        data.positions = new List<Position>();
        data.achievements = new List<IdItem>();
        data.inventory = new List<IdItem>();
    }
}
