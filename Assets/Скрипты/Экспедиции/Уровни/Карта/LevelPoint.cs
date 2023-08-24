using UnityEngine;

[CreateAssetMenu(fileName = "Point", menuName = "Expeditions/Level/Level Point")]
public class LevelPoint : GUIDScriptableObject, System.IEquatable<LevelPoint>
{
    public Vector2 position;
    public LevelPoint[] connectedPoints;

    public bool Equals(LevelPoint obj) =>
        obj.id == id;
}
