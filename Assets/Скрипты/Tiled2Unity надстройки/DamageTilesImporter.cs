using UnityEngine;
using System.Linq;
using SuperTiled2Unity;
using SuperTiled2Unity.Editor;

[AutoCustomTmxImporter]
public class DamageTilesImporter : CustomTmxImporter
{
    public override void TmxAssetImported(TmxAssetImportedArgs args)
    {
        var damageTiles = args.ImportedSuperMap.GetComponentsInChildren<SuperTileLayer>().First(o =>
            o.GetComponent<SuperCustomProperties>().TryGetCustomProperty("damageable", out var prop));

        foreach (var i in damageTiles.GetComponentsInChildren<PolygonCollider2D>()) 
        {
            i.isTrigger = true;

            var damage = i.gameObject.AddComponent<Damageable>();
            var trigger = i.gameObject.AddComponent<Trigger>();

            trigger.PlayerMask = 1 << LayerMask.NameToLayer("Player");
            trigger.action.AddListener(damage.Damage);
        }
    }
}