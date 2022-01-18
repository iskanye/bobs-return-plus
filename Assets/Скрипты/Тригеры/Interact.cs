using UnityEngine;

//Скрипт для обьектов, с которыми можно взаимодейвствовать
public class Interact : MonoBehaviour
{
    public LayerMask playerMask; //Слой игрока
    public UnityEngine.Events.UnityEvent action; //Действиее при взаимодействии

    void OnTriggerStay2D(Collider2D c) 
    {
        //Если игрок находится в зоне триггера и он нажал спейс, то мы выполняем действие выше
        if (1 << c.gameObject.layer == playerMask.value && Input.GetKeyDown(KeyCode.Space)) action.Invoke(); 
    }
}
