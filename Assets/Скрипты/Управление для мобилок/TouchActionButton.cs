using UnityEngine;

public class TouchActionButton : MonoBehaviour
{
    public void Press() =>
        StartCoroutine(_Press());

    System.Collections.IEnumerator _Press() 
    {
        InputManager.Active.submit = true;
        yield return new WaitForEndOfFrame();
        InputManager.Active.submit = false; 
    }
}
