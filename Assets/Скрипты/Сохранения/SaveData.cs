using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

[XmlRoot("Data")]
public class SaveData
{
    [XmlAttribute("Level")]
    public int level;
    [XmlAttribute("Lives")]
    public int lives = 3;
    [XmlArray("Positions"), XmlArrayItem("Position")]
    public List<Position> positionData;

    [XmlArray("Properties"), XmlArrayItem("Property")]
    public List<CustomProperty> customProperties;

    public SaveData() { }
}

[XmlType("PositionData")]
public class Position 
{
    [XmlAttribute("Object")]
    public string objectName;

    [XmlElement("Position")]
    public Vector3 position;

    public Position() { }

    public Position(string name, Vector3 position)
    {
        objectName = name;
        this.position = position;
    }
}

[XmlType("PropertyData")]
public class CustomProperty
{
    [XmlAttribute("Object")]
    public string objectName;

    [XmlAttribute("Property")]
    public bool property;

    public CustomProperty() { }

    public CustomProperty(string name, bool property)
    {
        objectName = name;
        this.property = property;
    }
}