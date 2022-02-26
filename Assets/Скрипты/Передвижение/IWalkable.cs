using UnityEngine;

public interface IWalkable 
{
    bool IsWalking { get; }
    Vector2 Direction { get; }
}
