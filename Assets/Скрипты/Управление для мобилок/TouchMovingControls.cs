using UnityEngine;

public class TouchMovingControls : MonoBehaviour
{
    public GameObject joystick;
    public GameObject dpad;

    public bool IsDPad 
    {
        set 
        {
            joystick.SetActive(!value);
            dpad.SetActive(value); 
        }
    }
}
