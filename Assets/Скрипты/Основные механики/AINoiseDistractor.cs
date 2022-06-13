using UnityEngine;

//Скрипт создания скриптовых шумов для ИИ
public class AINoiseDistractor : MonoBehaviour
{
    public float range; //Радиус дейсвия шума
    public Vector3 offset;
    public LayerMask AIMask; //Слой ИИ

    //Функция шума
    public void Distract()
    {
        var trans = transform;
        trans.position += offset;

        var AIs = Physics2D.OverlapCircleAll(trans.position, range, AIMask); //Ищем в радиусе ИИ
        //Если такие есть то мы их уведомляем о начилии шума
        foreach (var i in AIs)
            i.GetComponent<AIManager>().NoiseDistraction(trans); 
    }

    //Визуализируем шум
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + offset, range); //Показываем радиус действия шума желтым кругом
    }
}
