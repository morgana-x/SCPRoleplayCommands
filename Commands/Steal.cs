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

namespace FunCommands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    
    public class Steal : ParentCommand
    {

        public Steal() => LoadGeneratedCommands();

        public override string Command => ".steal";

        public override string[] Aliases => new string[] { "steal", ".steal" };

        public override string Description => "has a chance of stealing an item from someone infront of you";

        public override void LoadGeneratedCommands() { }

        public static Dictionary<Exiled.API.Features.Player, DateTime> Cooldowns = new Dictionary<Exiled.API.Features.Player, DateTime>();

        public System.Random rnd = new System.Random(); 
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Plugin.Instance.Config.StealEnabled)
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


            if (!Physics.Raycast(ray, out RaycastHit hit, Plugin.Instance.Config.StealRange))
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

            if (!Cooldowns.TryGetValue(Instigator, out _))
            {
                Cooldowns.Add(Instigator, DateTime.Now.AddSeconds(Plugin.Instance.Config.StealCooldown));
            }
            else
            {
                Cooldowns[Instigator] = DateTime.Now.AddSeconds(Plugin.Instance.Config.StealCooldown);
            }

            StealPlayer(Instigator, Victim);

            response = "true";
            return true;
        }
        private void StealPlayer(Exiled.API.Features.Player Instigator, Exiled.API.Features.Player Victim)
        {
            if (Victim.Inventory.UserInventory.Items.Count <= 0)
            {
                Instigator.ShowHint("\n" + Plugin.Instance.Config.StealEmptyInventoryHint.Replace("{player}", Victim.DisplayNickname).Replace("{rolecolor}", Victim.Role.Color.ToHex()), duration: Plugin.Instance.Config.StealHintDuration);
                return;
            }
            if (rnd.NextDouble() > Plugin.Instance.Config.StealChance)
            {
                Victim.ShowHint("\n" + Plugin.Instance.Config.StealFailHintVictim.Replace("{player}", Instigator.DisplayNickname).Replace("{rolecolor}", Instigator.Role.Color.ToHex()), duration: Plugin.Instance.Config.StealHintDuration);
                Instigator.ShowHint("\n" + Plugin.Instance.Config.StealFailHintInstigator.Replace("{player}", Victim.DisplayNickname).Replace("{rolecolor}", Victim.Role.Color.ToHex()), duration: Plugin.Instance.Config.StealHintDuration);
                return;
            }

            
            KeyValuePair<ushort, InventorySystem.Items.ItemBase> SelectedItem = Victim.Inventory.UserInventory.Items.GetRandomValue();
            ItemType item = SelectedItem.Value.ItemTypeId;
            Victim.Inventory.ServerRemoveItem(SelectedItem.Key, SelectedItem.Value.PickupDropModel);
            Instigator.AddItem(item);
            Victim.ShowHint("\n" + Plugin.Instance.Config.StealSuccessHintVictim.Replace("{player}", Instigator.DisplayNickname).Replace("{rolecolor}", Instigator.Role.Color.ToHex()).Replace("{item}", item.ToString()), duration: Plugin.Instance.Config.StealHintDuration);
            Instigator.ShowHint("\n" + Plugin.Instance.Config.StealSuccessHintInstigator.Replace("{player}", Victim.DisplayNickname).Replace("{rolecolor}", Victim.Role.Color.ToHex()).Replace("{item}", item.ToString()), duration: Plugin.Instance.Config.StealHintDuration);


        }
    }
}
