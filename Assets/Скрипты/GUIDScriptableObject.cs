public abstract class GUIDScriptableObject : UnityEngine.ScriptableObject
{
    public string id;
    
    public void Reset() => 
        id = System.Guid.NewGuid().ToString();
}
