using UnityEngine;
using UnityEngine.InputSystem;

public class RebindSaveLoad : MonoBehaviour
{
    InputActionAsset Actions => InputManager.Input.asset;

    void OnEnable()
    {
        if (!PlayerPrefs.HasKey("rebinds"))
            return;

        Actions.LoadBindingOverridesFromJson(PlayerPrefs.GetString("rebinds"));
    }

    void OnDisable()
    {
        var rebinds = Actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }
}
