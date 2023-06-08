using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public (UnityEngine.Events.UnityEvent<GameObject>, Transform) currentInteraction = (null, null);

    void Update()
    {
        if (currentInteraction.Item2 != null && TopDownMovement.isEnabled && InputManager.Active.submit)
            currentInteraction.Item1?.Invoke(gameObject);
    }
}
