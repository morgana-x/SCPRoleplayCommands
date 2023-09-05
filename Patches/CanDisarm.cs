namespace RoleplayCommands.Patches
{
    using HarmonyLib;
    using InventorySystem.Items;
    using InventorySystem;
    //using Mirror;
    using PlayerRoles;
    using PlayerRoles.FirstPersonControl;
    using UnityEngine;
    //using Utils.Networking;
    using InventorySystem.Disarming;
    using Exiled.API.Features;
    using static InventorySystem.Disarming.DisarmedPlayers;

    [HarmonyPatch(typeof(DisarmedPlayers), nameof(DisarmedPlayers.ValidateEntry))]
    public static class Patch
    {
        public static bool Prefix(DisarmedPlayers.DisarmedEntry entry, ref bool __result)
        {
            __result = false;
            if (entry.Disarmer == 0U)
            {
                __result = true;
                return true;
            }
            ReferenceHub referenceHub;
            if (!ReferenceHub.TryGetHubNetID(entry.DisarmedPlayer, out referenceHub))
            {
                return false;
            }
            ReferenceHub referenceHub2;
            if (!ReferenceHub.TryGetHubNetID(entry.Disarmer, out referenceHub2))
            {
                return false;
            }
            if (!referenceHub.IsHuman() || !referenceHub2.IsHuman())
            {
                return false;
            }
            Vector3 position = (referenceHub.roleManager.CurrentRole as IFpcRole).FpcModule.Position;
            Vector3 position2 = (referenceHub2.roleManager.CurrentRole as IFpcRole).FpcModule.Position;
            if ((position - position2).sqrMagnitude > 8100f)
            {
                return false;
            }
            if (referenceHub.GetFaction() == referenceHub2.GetFaction())
            {
                referenceHub.inventory.ServerDropEverything();
                __result = true;
                return false;
            }
            referenceHub.inventory.ServerDropEverything();
            __result = true;
            return false;
        }
    }
}
