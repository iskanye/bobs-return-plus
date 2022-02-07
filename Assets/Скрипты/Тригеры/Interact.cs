using UnityEngine;

//Скрипт для обьектов, с которыми можно взаимодейвствовать
public class Interact : ActionBase
{   
    public LayerMask playerMask; //Слой игрока

    bool trigger;
    GameObject player;

    void Start() =>
        InputManager.Active.AddListenerToActionStarted("Submit", e => 
        {
            if (trigger)
                action.Invoke(player); 
        });

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
}
