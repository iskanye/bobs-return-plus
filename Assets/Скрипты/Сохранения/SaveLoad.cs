using UnityEngine;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Collections.Generic;

public class SaveLoad
{
    public async static void Save(SaveData data)
    {
        Type[] customTypes = { typeof(Position), typeof(CustomProperty) };
        var root = new XmlSerializer(typeof(SaveData), customTypes);

        using (var file = new FileStream(Application.dataPath + "/Saves/save.xml", FileMode.Create)) await Task.Run(() => root.Serialize(file, data));
    }

    public static SaveData Load()
    {
        Type[] customTypes = { typeof(Position), typeof(CustomProperty) };
        var root = new XmlSerializer(typeof(SaveData), customTypes);
        SaveData result;
        result = new SaveData();

        try
        {
            using (var file = new FileStream(Application.dataPath + "/Saves/save.xml", FileMode.OpenOrCreate)) result = (SaveData)root.Deserialize(file);
        }

        catch
        {
            result.customProperties = new List<CustomProperty>();
            result.positionData = new List<Position>();
            result.level = 0;
        }

        return result;
    }

    public async static void SaveAchievement(AchievementJson[] achievements)
    {
        using (var file = new StreamWriter(Application.dataPath + "/Saves/achievements.json", false)) await file.WriteLineAsync(JsonHelper.ToJson(achievements));
    }

    public static AchievementJson[] GetAchievements()
    {
        AchievementJson[] result;

        try
        {
            using (var file = new StreamReader(Application.dataPath + "/Saves/achievements.json")) result = JsonHelper.FromJson<AchievementJson>(file.ReadToEnd());
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
