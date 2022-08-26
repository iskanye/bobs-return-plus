using UnityEngine;
using System.Collections;

public class SceneData : MonoBehaviour
{
    public TMPro.TMP_Text savingText;

    public static SaveData Data { get; private set; }

    public static SceneData Active { get; private set; }

    PositionHolder[] positions;
    PropertyHolder[] properties;
    IntegerHolder[] integers;
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
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += (i, j) => Start();
    }

    void Start()
    {
        positions = FindObjectsOfType<PositionHolder>();
        properties = FindObjectsOfType<PropertyHolder>();
        integers = FindObjectsOfType<IntegerHolder>();
        lives = FindObjectOfType<PlayerLiveCounter>();
        inventory = FindObjectOfType<InventorySystem>();
    }

    public static void Save()
    {
        var active = Active;
        var temp = Data.achievements;

        Data = new SaveData
        {
            level = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex,
            lives = active.lives.LivesRemaining,
            achievements = temp
        };

        foreach (var i in active.positions) 
            Data.positions.Add(new Position(i.id, i.gameObject.transform.position));

        foreach (var i in active.properties) 
            Data.customProperties.Add(new CustomProperty(i.id, i.property));

        foreach (var i in active.integers)
            Data.integers.Add(new Integer(i.id, i.Integer.integer));

        foreach (var i in active.inventory.items)
            if (i != null) 
                Data.inventory.Add(new GuidItem(i.id));

        active.StopAllCoroutines();

        if (active.savingText != null)
            active.StartCoroutine(active.Saving());
    }

    public static void DeleteSaves()
    {
        var active = Active;

        active.StopAllCoroutines();

        if (active.savingText != null)
            active.StartCoroutine(active.Deleting());
    }

    IEnumerator Saving()
    {
        SaveLoad.Save(Data);

        savingText.text = "сохранено";
        yield return new WaitForSecondsRealtime(3);
        savingText.text = "";
    }

    IEnumerator Deleting()
    {
        SaveLoad.DeleteSaves();
        Data = new SaveData();

        savingText.text = "удалено";
        yield return new WaitForSecondsRealtime(3);
        savingText.text = "";
    }
}
