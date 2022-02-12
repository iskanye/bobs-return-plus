using UnityEngine;

//Скрипт для обьектов, с которыми можно взаимодейвствовать
public class Interact : ActionBase, IInputListener
{   
    public LayerMask playerMask; //Слой игрока

    bool trigger;
    GameObject player;

    void Start() =>
        InputManager.AddListenerToActionStarted("Submit", Input);

    void OnTriggerStay2D(Collider2D c)
    {
        if (((1 << c.gameObject.layer) | playerMask) == playerMask) 
        {
            trigger = true;
            player = c.gameObject; 
        }
    }

    void OnTriggerExit2D(Collider2D c) => 
        trigger = false;

    public void DeleteListeners() =>
        InputManager.RemoveListenerAtActionStarted("Submit", Input);

    void Input(UnityEngine.InputSystem.InputAction.CallbackContext e) 
    {
        if (trigger)
            action.Invoke(player);
    }
}
