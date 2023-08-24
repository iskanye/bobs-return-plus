using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Expeditions/Level/Level")]
public class ExpeditionLevel : ScriptableObject
{
    public Sprite icon;
    public string episodeTitle;
    public string title;
    public string briefDescription;
    public LevelPoint rootPoint;

}
