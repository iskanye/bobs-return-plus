using UnityEngine;

//Контроллер анимаций для движущихся обьектов
public class AnimationMovementController : MonoBehaviour
{
    Animator animator; //Контроллер анимаций от Unity
    IWalkable movingObject;

    void Awake()
    {
        animator = GetComponent<Animator>(); //Ищем этот контроллер
        movingObject = GetComponent<IWalkable>();
    }

    void Update()
    {
        bool isWalking = movingObject.IsWalking;
        animator.SetBool("Is Walk", isWalking); //Говорим контроллеру, когда мы двигаемся

        if (isWalking)
        {
            animator.SetFloat("Direction X", movingObject.Direction.x);
            animator.SetFloat("Direction Y", movingObject.Direction.y);
        }
    }
}


