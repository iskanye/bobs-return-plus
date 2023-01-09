public class PropertyHolder : GUIDHolder
{
    public bool isLocal = true;
    public bool property { set => SceneData.SetProperty(id, value, isLocal); }
    public UnityEngine.Events.UnityEvent<bool> action;

    void Start() 
    {
        if (SceneData.HasProperty(id, isLocal)) 
            action.Invoke(SceneData.GetProperty(id, isLocal)); 
    }
}
