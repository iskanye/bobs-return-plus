using UnityEngine;

//Скрипт для обьектов, с которыми можно взаимодейвствовать
public class Interact : ActionBase
{
    public LayerMask playerMask; //Слой игрока

    void OnTriggerStay2D(Collider2D c) 
    {
        //Если игрок находится в зоне триггера и он нажал спейс, то мы выполняем действие выше
        if (((1 << c.gameObject.layer) | playerMask) == playerMask && Input.GetKeyDown(KeyCode.Space)) action.Invoke(c.gameObject); 
    }
}
