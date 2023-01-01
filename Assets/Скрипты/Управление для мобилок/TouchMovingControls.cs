using UnityEngine;

public class TouchMovingControls : MonoBehaviour
{
    public GameObject joystick;
    public GameObject dpad;

    public bool IsDPad 
    {
        set 
        {
            PlayerPrefs.SetInt("DPad", value ? 1 : 0);
            joystick.SetActive(!value);
            dpad.SetActive(value); 
        }
    }

    void Start() 
    {
        if (PlayerPrefs.HasKey("DPad"))
            IsDPad = PlayerPrefs.GetInt("DPad") == 1;
    }
}
