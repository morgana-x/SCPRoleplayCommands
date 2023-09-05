using CommandSystem;
using Exiled.API.Features;
using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using MEC;
using Exiled.API.Features.Items;
using Exiled.API.Extensions;
using InventorySystem;
using InventorySystem.Items;

namespace FunCommands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    
    public class Cuff : ParentCommand
    {

        public Cuff() => LoadGeneratedCommands();

        public override string Command => ".cuff";

        public override string[] Aliases => new string[] { "cuff", ".cuff" };

        public override string Description => "allows you to cuff teammates";

        public override void LoadGeneratedCommands() { }

        public static Dictionary<Exiled.API.Features.Player, DateTime> Cooldowns = new Dictionary<Exiled.API.Features.Player, DateTime>();

        public System.Random rnd = new System.Random(); 

        private bool IsHoldingValidCuffWeapon(Player Disarmer)
        {
            ReferenceHub disarmerHub = Disarmer.ReferenceHub;
            ItemBase curInstance = disarmerHub.inventory.CurInstance;
            if (curInstance != null)
            {
                IDisarmingItem disarmingItem = curInstance as IDisarmingItem;
                if (disarmingItem != null)
                {
                    return disarmingItem.AllowDisarming;
                }
            }
            return false;
        }
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Plugin.Instance.Config.CuffEnabled)
            {
                response = "Command disabled";
                return false;
            }
            Exiled.API.Features.Player Instigator = Exiled.API.Features.Player.Get(sender);
            if (Instigator.IsCuffed)
            {
                Instigator.ShowHint("\n" + Plugin.Instance.Config.CuffedHintText);
                response = Plugin.Instance.Config.CuffedHintText;
                return false;
            }
            if (Cooldowns.TryGetValue(Instigator, out DateTime value))
            {
                if (DateTime.Now < value)
                {
                    Instigator.ShowHint("\n" + Plugin.Instance.Config.CooldownHintText.Replace("{rolecolor}", Instigator.Role.Color.ToHex()).Replace("{time}", Math.Ceiling(value.Subtract(DateTime.Now).TotalSeconds).ToString()));
                   // Instigator.ShowHint("Wait <color="+  Instigator.Role.Color.ToHex() + ">" + (Math.Ceiling(value.Subtract(DateTime.Now).TotalSeconds).ToString()) + "</color> seconds before using this again.");
                    response = "Cooldown active";
                    return false;
                }
            }

            var ray = new Ray(Instigator.CameraTransform.position + (Instigator.CameraTransform.forward * 0.1f), Instigator.CameraTransform.forward);


            if (!Physics.Raycast(ray, out RaycastHit hit, Plugin.Instance.Config.CuffRange))
            {
                response = "";
                return false;
            }


            var Victim = Exiled.API.Features.Player.Get(hit.collider);
            if (Victim == null)
            {
                response = "";
                return false;
            }
            if ( Victim == Instigator)
            {
                response = "";
                return false;
            }
            if (Victim.IsScp)
            {
                response = "";
                return false;
            }
            if (Victim.IsCuffed)
            {
                Instigator.ShowHint(Plugin.Instance.Config.CuffHintAlreadyCuffed.Replace("{player}", Victim.DisplayNickname).Replace("{rolecolor}", Victim.Role.Color.ToHex()));
                response = "";
                return false;
            }
            if (Plugin.Instance.Config.CuffSameTeamOnly && (Victim.Role.Side != Instigator.Role.Side))
            {
                Instigator.ShowHint(Plugin.Instance.Config.CuffSameTeamOnlyHint, duration: Plugin.Instance.Config.CuffHintDuration);
                response = "";
                return false;
            }
            if (Plugin.Instance.Config.CuffRequireFirearm && (!IsHoldingValidCuffWeapon(Instigator)))
            {
                Instigator.ShowHint(Plugin.Instance.Config.CuffRequireFirearmHint, duration: Plugin.Instance.Config.CuffHintDuration);
                response = "";
                return false;
            }

            

            if (!Cooldowns.TryGetValue(Instigator, out _))
            {
                Cooldowns.Add(Instigator, DateTime.Now.AddSeconds(Plugin.Instance.Config.CuffCooldown));
            }
            else
            {
                Cooldowns[Instigator] = DateTime.Now.AddSeconds(Plugin.Instance.Config.CuffCooldown);
            }

            CuffPlayer(Instigator, Victim);
            response = "";
            return true;
        }
        private void CuffPlayer(Exiled.API.Features.Player Instigator, Exiled.API.Features.Player Victim)
        {
            // Victim.Handcuff(Instigator);
            Victim.Cuffer = Instigator;
            Instigator.ShowHint("\n" + Plugin.Instance.Config.CuffHintInstigator.Replace("{player}", Victim.DisplayNickname).Replace("{rolecolor}", Victim.Role.Color.ToHex()), duration: Plugin.Instance.Config.CuffHintDuration);
        }
    }
}
