using UnityEngine.Events;

public class PropertyHolder : GUIDHolder
{
    public bool isLocal = true;
    public bool invokeOnUpdate;
    public bool property { set => SceneData.SetProperty(id, value, isLocal); }
    public UnityEvent<bool> action;

    void Start() 
    {
        if (SceneData.HasProperty(id, isLocal)) 
            action.Invoke(SceneData.GetProperty(id, isLocal)); 

        if (invokeOnUpdate)
            if (SceneData.OnPropertySet.ContainsKey(id))
                SceneData.OnPropertySet[id].Add(action);
            else
                SceneData.OnPropertySet[id] = new System.Collections.Generic.List<UnityEvent<bool>>() { action };
    }
}
