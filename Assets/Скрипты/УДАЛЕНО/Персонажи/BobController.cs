using UnityEngine;

public class BobController : MonoBehaviour
{
    public PlayerData data;
    public BaseBob character;

    void Start() =>
        Enable();

    public void Disable()
    {
        StopAllCoroutines();
        data.movement.Disable();
        character.Stop();
    }

    public void Enable()
    {      
        data.movement.Enable();
        character.data = data;
        character.controller = this;
        StartCoroutine(character.Start());
    }

    public void SetCharacterBool(string name) =>
        character.SetBool(name);
}
