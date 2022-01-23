using UnityEngine;

//Скрипт создания скриптовых шумов для ИИ
public class AINoiseDistractor : MonoBehaviour
{
    public float range; //Радиус дейсвия шума
    public LayerMask AIMask; //СЛой ИИ

    //Функция шума
    public void Distract()
    {
        var AIs = Physics2D.OverlapCircleAll(transform.position, range, AIMask); //Ищем в радиусе ИИ
        //Если такие есть то мы их уведомляем о начилии шума
        foreach (var i in AIs) i.GetComponent<AIManager>().NoiseDistraction(transform); 
    }

    //Визуализируем шум
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range); //Показываем радиус действия шума желтым кругом
    }
}
