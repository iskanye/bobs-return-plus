using UnityEngine;

//Скрипт для обьектов, с которыми можно взаимодейвствовать
public class Interact : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent action; //Действиее при взаимодействии

    void OnTriggerStay2D(Collider2D c) 
    {
        //Если игрок находится в зоне триггера и он нажал спейс, то мы выполняем действие выше
        if (c.CompareTag("Player") && Input.GetKeyDown(KeyCode.Space)) action.Invoke(); 
    }
}
