using CommandSystem;
using Exiled.API.Features;
using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using MEC;

namespace FunCommands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    
    public class Push : ParentCommand
    {

        public Push() => LoadGeneratedCommands();

        public override string Command => ".push";

        public override string[] Aliases => new string[] { "push", ".p" };

        public override string Description => "pushes someone in front of you.";

        public override void LoadGeneratedCommands() { }

        public static Dictionary<Exiled.API.Features.Player, DateTime> Cooldowns = new Dictionary<Exiled.API.Features.Player, DateTime>();

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Plugin.Instance.Config.PushEnabled)
            {
                response = "Command disabled";
                return false;
            }
            Exiled.API.Features.Player Instigator = Exiled.API.Features.Player.Get(sender);

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


            if (!Physics.Raycast(ray, out RaycastHit hit, Plugin.Instance.Config.PushRange))
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

            if (!Cooldowns.TryGetValue(Instigator, out _))
            {
                Cooldowns.Add(Instigator, DateTime.Now.AddSeconds(Plugin.Instance.Config.PushCooldown));
            }
            else
            {
                Cooldowns[Instigator] = DateTime.Now.AddSeconds(Plugin.Instance.Config.PushCooldown);
            }

            Timing.RunCoroutine(PushPlayer(Instigator, Victim));


            Victim.ShowHint("\n" + Plugin.Instance.Config.PushHintVictim.Replace("{player}", Instigator.DisplayNickname).Replace("{rolecolor}", Instigator.Role.Color.ToHex()));
            Instigator.ShowHint("\n" + Plugin.Instance.Config.PushHintInstigator.Replace("{player}", Victim.DisplayNickname).Replace("{rolecolor}", Victim.Role.Color.ToHex() ));


            response = "true";
            return true;
        }
        private IEnumerator<float> PushPlayer(Exiled.API.Features.Player Instigator, Exiled.API.Features.Player Victim)
        {
            Vector3 pushed = Instigator.CameraTransform.forward * Plugin.Instance.Config.PushForce;
            Vector3 endPos = Victim.Position + new Vector3(pushed.x, 0, pushed.z);
            int layerAsLayerMask = 0;
            for (int x = 1; x < 8; x++)
                layerAsLayerMask |= (1 << x);
            for (int i = 1; i < Plugin.Instance.Config.Iterations; i++)
            {

                float movementAmount = Plugin.Instance.Config.PushForce / Plugin.Instance.Config.Iterations;


                Vector3 newPos = Vector3.MoveTowards(Victim.Position, endPos, movementAmount);

                if (Physics.Linecast(Victim.Position, newPos, layerAsLayerMask))
                    yield break;

                Victim.Position = newPos;


                yield return Timing.WaitForOneFrame;
            }

        }
    }
}
