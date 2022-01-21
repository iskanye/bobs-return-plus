using UnityEngine;

public class IfPlayerPrefsVariableExists : MonoBehaviour
{
    public string variableName;
    public UnityEngine.Events.UnityEvent action;
    void Awake() 
    {
        if (PlayerPrefs.HasKey(variableName)) action.Invoke();
    }
}
