using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static UnityEngine.SceneManagement.SceneManager;

public class SceneData : MonoBehaviour
{
    public TMPro.TMP_Text savingText;

    public static SaveData Data { get; private set; }

    public static SceneData Active { get; private set; }

    public static Dictionary<string, List<UnityEngine.Events.UnityEvent<bool>>> OnPropertySet { get; set; }

    PositionHolder[] positions;
    IntegerHolder[] integers;
    PlayerLiveCounter lives;
    InventorySystem inventory;

    void Awake()
    {
        Active = this;
        
        OnPropertySet = new Dictionary<string, List<UnityEngine.Events.UnityEvent<bool>>>();
        sceneLoaded += (i, j) => 
        {
            Start();
            Load();
        };
        sceneUnloaded += i => OnPropertySet.Clear();
        Load();
    }

    void Start()
    {     
        positions = FindObjectsOfType<PositionHolder>();
        integers = FindObjectsOfType<IntegerHolder>();
        lives = PlayerLiveCounter.Active;
        inventory = FindObjectOfType<InventorySystem>();
    }

    void Load()
    {
        Data = SaveLoad.Load();
        
        if (Data.version != SaveData.currentVersion)
            Data = new SaveData();

        if (Data.level != GetActiveScene().buildIndex)
        { 
            Data.localProperties = new List<Property<bool>>();           
            Data.positions = new List<Property<Vector3>>();
            Data.integers = new List<Property<int>>();
            Data.inventory = new List<GuidItem>();
        }
    }

    public static bool HasProperty(string id, bool local = true) => 
        id != "" && (local ? Data.localProperties.Exists(i => i.id == id) : Data.globalProperties.Exists(i => i.id == id));    

    public static void SetProperty(string id, bool property, bool local = true) 
    {
        if (local) 
        {
            if (HasProperty(id))
                Data.localProperties.Find(i => i.id == id).property = property;

            else 
                Data.localProperties.Add(new Property<bool>(id, property));
        }

        else 
        {
            if (HasProperty(id, false))
                Data.globalProperties.Find(i => i.id == id).property = property;

            else 
                Data.globalProperties.Add(new Property<bool>(id, property));
        }

        if (OnPropertySet.ContainsKey(id))
            OnPropertySet[id].ForEach(e => e.Invoke(property));
    } 

    public static bool GetProperty(string id, bool local = true)
    {
        if (!HasProperty(id, local))
            return false;

        return local ? Data.localProperties.Find(i => i.id == id).property : Data.globalProperties.Find(i => i.id == id).property;
    }

    public static void Save()
    {
        Data.level = GetActiveScene().buildIndex;
        Data.lives = Active.lives.LivesRemaining;
        Data.positions = new List<Property<Vector3>>();
        Data.integers = new List<Property<int>>();
        Data.inventory = new List<GuidItem>();

        foreach (var i in Active.positions) 
            Data.positions.Add(new Property<Vector3>(i.id, i.gameObject.transform.position));

        foreach (var i in Active.integers)
            Data.integers.Add(new Property<int>(i.id, i.Integer.integer));

        foreach (var i in Active.inventory.items)
            if (i != null) 
                Data.inventory.Add(new GuidItem(i.id));
        
        SaveLoad.Save(Data);

        Active.StopAllCoroutines();

        if (Active.savingText != null)
            Active.StartCoroutine(Active.Saving());
    }
    
    public static void DeleteSaves()
    {
        var active = Active;
        
        SaveLoad.DeleteSaves();

        var temp = Data.globalProperties;
        Data = new SaveData();

        active.StopAllCoroutines();

        if (active.savingText != null)
            active.StartCoroutine(active.Deleting());
    }

    IEnumerator Saving()
    {
        savingText.text = "сохранено";
        yield return new WaitForSecondsRealtime(3);
        savingText.text = "";
    }

    IEnumerator Deleting()
    {
        savingText.text = "удалено";
        yield return new WaitForSecondsRealtime(3);
        savingText.text = "";
    }
}
