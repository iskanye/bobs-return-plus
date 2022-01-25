using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

public class SaveLoad
{
    public async static void Save(SaveData data)
    {
        using (var file = new StreamWriter(Application.dataPath + "/Saves/save.json", false)) await file.WriteLineAsync(JsonUtility.ToJson(data));
    }

    public static SaveData Load()
    {
        SaveData result;
        result = new SaveData();

        try
        {
            using (var file = new StreamReader(Application.dataPath + "/Saves/save.json")) result = JsonUtility.FromJson<SaveData>(file.ReadToEnd());
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

    public async static void SaveJson(IdItem[] data, string fileName)
    {
        using (var file = new StreamWriter(Application.dataPath + "/Saves/" + fileName + ".json", false)) await file.WriteLineAsync(JsonHelper.ToJson(data));
    }

    public static IdItem[] LoadJson(string fileName)
    {
        IdItem[] result;

        try
        {
            using (var file = new StreamReader(Application.dataPath + "/Saves/" + fileName + ".json")) result = JsonHelper.FromJson<IdItem>(file.ReadToEnd());
        }

        catch
        {
            result = null;
        }
        
        return result;
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
