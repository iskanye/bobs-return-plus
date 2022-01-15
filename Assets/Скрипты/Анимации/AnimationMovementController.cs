using UnityEngine;

//Контроллер анимаций для движущихся обьектов
public class AnimationMovementController : MonoBehaviour
{
    internal bool isWalk; //Двигает ли обьект?
    internal Vector2 direction; //Направления движения

    Animator anim; //Контроллер анимаций от Unity

    void Awake() => anim = GetComponent<Animator>(); //Ищем этот контроллер

    void Update()
    {
        anim.SetBool("Is Walk", isWalk); //Говорим контроллеру, когда мы двигаемся

        if (isWalk) //Если двигаемся отправляем ему данные о нашем направлении
        {
            anim.SetFloat("Direction X", direction.x);        
            anim.SetFloat("Direction Y", direction.y);
        }
    }
}
