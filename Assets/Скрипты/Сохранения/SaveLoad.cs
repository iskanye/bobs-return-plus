using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveLoad
{
    public async static void Save(SaveData data)
    {
        using (var file = new StreamWriter(Application.persistentDataPath + "/save.json", false)) await file.WriteLineAsync(JsonUtility.ToJson(data));
    }

    public static SaveData Load()
    {
        SaveData result;
        result = new SaveData();

        try
        {
            using (var file = new StreamReader(Application.persistentDataPath + "/save.json")) result = JsonUtility.FromJson<SaveData>(file.ReadToEnd());
        }

        catch
        {
            result.customProperties = new List<CustomProperty>();
            result.positionData = new List<Position>();
            result.achievements = new List<IdItem>();
            result.inventory = new List<IdItem>();
            result.level = 0;
        }

        return result;
    }
}
