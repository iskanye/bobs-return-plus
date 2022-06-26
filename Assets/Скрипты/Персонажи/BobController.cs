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
}
