using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Classic", menuName = "Bobs/Classic", order = 0)]
public class BaseBob : ScriptableObject
{
    [Range(0, 10)] public int speed;
    protected BobType type;

    [HideInInspector] public PlayerData data;
    [HideInInspector] public BobController controller;

    public virtual IEnumerator Start()
    {
        data.movement.speed = speed;
        controller.StartCoroutine(Process());
        yield return null;
    }

    public virtual IEnumerator Process() 
    {
        yield return null;
    }
}

public enum BobType
{
    Fight,
    Support,
    Research
}
