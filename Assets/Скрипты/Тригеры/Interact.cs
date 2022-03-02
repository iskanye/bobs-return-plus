using UnityEngine;

//Скрипт для обьектов, с которыми можно взаимодейвствовать
public class Interact : ActionBase
{   
    public LayerMask playerMask; //Слой игрока

    bool trigger;
    GameObject player;

    void Awake() =>
        InputManager.AddListenerToActionStarted("Submit", Input);

    void OnTriggerStay2D(Collider2D c)
    {
        if (action != null && ((1 << c.gameObject.layer) | playerMask) == playerMask) 
        {
            trigger = true;
            player = c.gameObject; 
        }
    }

    void OnTriggerExit2D(Collider2D c) => 
        trigger = false;

    void Input(UnityEngine.InputSystem.InputAction.CallbackContext e) 
    {
        if (trigger)
            action.Invoke(player);
    }
}
