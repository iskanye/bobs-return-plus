using UnityEngine;

//Скрипт для обьектов, с которыми можно взаимодейвствовать
public class Interact : ActionBase
{   
    [SerializeField] private LayerMask playerMask; //Слой игрока

    void OnTriggerStay2D(Collider2D c)
    {
        if (action != null && ((1 << c.gameObject.layer) | playerMask) == playerMask && InputManager.Active.submit && TopDownMovement.isEnabled)
            action.Invoke(c.gameObject);
    }
}
