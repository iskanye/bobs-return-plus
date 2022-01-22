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
}
