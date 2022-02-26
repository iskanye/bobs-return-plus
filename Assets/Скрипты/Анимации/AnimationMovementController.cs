using System.Collections;
using UnityEngine;

//Контроллер анимаций для движущихся обьектов
public class AnimationMovementController : SequenceObject
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
        animator.SetBool("Is Walk", movingObject.IsWalking); //Говорим контроллеру, когда мы двигаемся
        animator.SetFloat("Direction X", movingObject.Direction.x);
        animator.SetFloat("Direction Y", movingObject.Direction.y);
    }

    public void Play(string animation) => 
        animator.Play(animation);

    public IEnumerator WaitForAnimationToStop(string animation)
    {
        var state = animator.GetCurrentAnimatorStateInfo(0);

        while (!state.IsName(animation))
        {
            state = animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }

        yield return new WaitForSeconds(state.length);
    }

    public override IEnumerator Sequence()
    {
        yield return WaitForAnimationToStop(animator.GetCurrentAnimatorStateInfo(0).ToString());
    }
}
