using System.Collections;
using UnityEngine;

//Контроллер анимаций для движущихся обьектов
public class AnimationMovementController : SequenceObject
{
    public BaseMovement movingObject;
    Animator animator; //Контроллер анимаций от Unity

    void Awake() =>
        animator = GetComponent<Animator>(); //Ищем этот контроллер

    void Update()
    {
        animator.SetBool("Is Walk", movingObject.IsWalking); //Говорим контроллеру, когда мы двигаемся
        animator.SetFloat("Direction X", movingObject.Direction.x);
        animator.SetFloat("Direction Y", movingObject.Direction.y);
    }

    public override IEnumerator Sequence()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); 
    }
}
