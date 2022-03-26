using System.IO;
using UnityEngine;

public static class SaveLoad
{
    public static void Save(SaveData data)
    {
        using (var file = new StreamWriter(Application.persistentDataPath + "/save.json", false)) 
            file.WriteLine(JsonUtility.ToJson(data));
    }

    public static SaveData Load()
    {
        var result = new SaveData();

        if (File.Exists(Application.persistentDataPath + "/save.json"))
            using (var file = new StreamReader(Application.persistentDataPath + "/save.json"))
                result = JsonUtility.FromJson<SaveData>(file.ReadToEnd());

        return result;
    }

    public static void DeleteSaves()
    {
        if (File.Exists(Application.persistentDataPath + "/save.json"))
            File.Delete(Application.persistentDataPath + "/save.json");
    }
}
