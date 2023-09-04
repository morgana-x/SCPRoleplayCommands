using CommandSystem;
using Exiled.API.Features;
using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using MEC;
using PlayerRoles;
using System.Linq;

namespace FunCommands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    
    public class Pat : ParentCommand
    {

        public Pat() => LoadGeneratedCommands();

        public override string Command => ".pat";

        public override string[] Aliases => new string[] { "pat", ".pat" };

        public override string Description => "pats scp 939";

        public override void LoadGeneratedCommands() { }

        public static Dictionary<Exiled.API.Features.Player, DateTime> Cooldowns = new Dictionary<Exiled.API.Features.Player, DateTime>();

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            if (!Plugin.Instance.Config.PatEnabled)
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
            if ((!Plugin.Instance.Config.PatCanSCPSPat) && (Instigator.Role.Side == Exiled.API.Enums.Side.Scp))
            {
                Instigator.ShowHint("\n" + Plugin.Instance.Config.PatHintSCPCantUse);
                response = "";
                return false;
            }

            if (Cooldowns.TryGetValue(Instigator, out DateTime value))
            {
                if (DateTime.Now < value)
                {
                    Instigator.ShowHint("\n" + Plugin.Instance.Config.CooldownHintText.Replace("{rolecolor}", Instigator.Role.Color.ToHex()).Replace("{time}", Math.Ceiling(value.Subtract(DateTime.Now).TotalSeconds).ToString()));
                    response = "";
                    return false;
                }
            }

            var ray = new Ray(Instigator.CameraTransform.position + (Instigator.CameraTransform.forward * 0.1f), Instigator.CameraTransform.forward);


            if (!Physics.Raycast(ray, out RaycastHit hit, Plugin.Instance.Config.PushRange))
            {
                response = "";
                return false;
            }


            var Victim = Exiled.API.Features.Player.Get(hit.collider);
            if ((Victim == null) || (Victim == Instigator))
            {
                response = "";
                return false;
            }

            if ((!Plugin.Instance.Config.PatAnyone) && (!Plugin.Instance.Config.PatableRoles.Contains(Victim.Role)))
            {
                Instigator.ShowHint("\n" + Plugin.Instance.Config.PatHintCantPatRole);
                response = "";
                return false;
            }
            if (!Cooldowns.TryGetValue(Instigator, out _))
            {
                Cooldowns.Add(Instigator, DateTime.Now.AddSeconds(Plugin.Instance.Config.PatCooldown));
            }
            else
            {
                Cooldowns[Instigator] = DateTime.Now.AddSeconds(Plugin.Instance.Config.PatCooldown);
            }

            PatPlayer(Victim);

            string VictimMsg = "\n" + Plugin.Instance.Config.PatHintVictim.Replace("{player}", Instigator.DisplayNickname).Replace("{hp}", Plugin.Instance.Config.PatHealthGrant.ToString()).Replace("{rolecolor}", Instigator.Role.Color.ToHex());
            string InstigatorMSG = "\n" + Plugin.Instance.Config.PatHintInstigator.Replace("{player}", Victim.DisplayNickname).Replace("{hp}", Plugin.Instance.Config.PatHealthGrant.ToString()).Replace("{rolecolor}", Victim.Role.Color.ToHex());
            Victim.ShowHint(VictimMsg, 6f);
            Instigator.ShowHint(InstigatorMSG, 6f);


            response = "true";
            return true;
        }
        private void PatPlayer(Exiled.API.Features.Player Victim)
        {
            Victim.Heal(Plugin.Instance.Config.PatHealthGrant, Plugin.Instance.Config.PatHealthExceedMax);
        }
    }
}
