using UnityEngine;
using System.Linq;
using SuperTiled2Unity;
using SuperTiled2Unity.Editor;

public class DamageTilesImporter : CustomTmxImporter
{
    public override void TmxAssetImported(TmxAssetImportedArgs args)
    {
        var damageTiles = args.ImportedSuperMap.GetComponentsInChildren<SuperTileLayer>().Where(o =>
            o.GetComponent<SuperCustomProperties>().TryGetCustomProperty("damageable", out var prop));

        foreach (var j in damageTiles)
            foreach (var i in j.GetComponentsInChildren<Collider2D>()) 
            {
                i.isTrigger = true;
                i.gameObject.AddComponent<Damageable>().addPersistentListener = true;

                var trigger = i.gameObject.AddComponent<Trigger>();
                trigger.PlayerMask = 1 << LayerMask.NameToLayer("Player");
                trigger.action.RemoveAllListeners();
            }
    }
}