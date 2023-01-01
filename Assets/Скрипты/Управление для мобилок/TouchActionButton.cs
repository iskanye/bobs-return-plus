using UnityEngine;

public class TouchActionButton : MonoBehaviour
{
    public void Press() =>
        StartCoroutine(_Press());

    System.Collections.IEnumerator _Press() 
    {
        InputManager.Active.submit = true;
        yield return new WaitForFixedUpdate();
        InputManager.Active.submit = false; 
    }

    public void Attack() =>
        StartCoroutine(_Attack());

    System.Collections.IEnumerator _Attack() 
    {
        InputManager.Active.attack = true;
        yield return new WaitForEndOfFrame();
        InputManager.Active.attack = false; 
    }
}
