using UnityEngine;

public class BobController : MonoBehaviour
{
    public PlayerData data;
    public BaseBob character;

    void Start() =>
        OnEnable();

    void OnDisable()
    {
        data.movement.enabled = false;
        StopAllCoroutines();
        character.Stop();
    }

    void OnEnable()
    {      
        data.movement.enabled = true;
        character.data = data;
        character.controller = this;
        StartCoroutine(character.Start());
    }

    public void SetCharacterBool(string name) =>
        character.SetBool(name);
}
