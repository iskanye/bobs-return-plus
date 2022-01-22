using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SceneData : MonoBehaviour
{
    public TMPro.TMP_Text savingText;

    PositionHolder[] positions;
    PropertyHolder[] properties;
    SaveData data;
    LiveCounter lives;

    void Start()
    {  
        positions = FindObjectsOfType<PositionHolder>();
        properties = FindObjectsOfType<PropertyHolder>();
        lives = FindObjectOfType<LiveCounter>();      
        data = SaveLoad.Load();

        foreach (var pos in data.positionData) positions.First(i => i.name == pos.objectName).gameObject.transform.position = pos.position;

        foreach (var prop in data.customProperties)
        {
            var pr = properties.First(i => i.name == prop.objectName);
            pr.property = prop.property;
            pr.action.Invoke(pr.property);
        }

        lives.livesRemaining = data.lives;

        for (int i = 0; i < lives.lives.Length; i++) lives.lives[i].gameObject.SetActive(i <= data.lives - 1);
    }

    public void Save() 
    {
        data.customProperties = new List<CustomProperty>();
        data.positionData = new List<Position>();
        data.level = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        data.lives = lives.livesRemaining;

        foreach (var i in positions) data.positionData.Add(new Position(i.name, i.gameObject.transform.position));

        foreach (var i in properties) data.customProperties.Add(new CustomProperty(i.name, i.property));

        SaveLoad.Save(data);
        StartCoroutine(Saved());
    }

    System.Collections.IEnumerator Saved()
    {
        savingText.text = "сохранено";
        yield return new WaitForSecondsRealtime(3);
        savingText.text = "";
    }
}
