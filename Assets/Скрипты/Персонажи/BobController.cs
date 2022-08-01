using UnityEngine;

public class BobController : MonoBehaviour
{
    public PlayerData data;
    public BaseBob character;

    void Start()
    {
        character.data = data;
        character.controller = this;
        StartCoroutine(character.Start());
    }

    void OnDisable()
    {
        data.movement.enabled = false;
        StopAllCoroutines();
        character.Stop();
    }

    void OnEnable()
    {
        data.movement.enabled = true;
        StartCoroutine(character.Start());
    }
}
