using Exiled.API.Interfaces;
using Exiled.API.Features;
using Exiled.API.Enums;
using System.ComponentModel;
using System.Collections.Generic;
using PlayerRoles;

namespace FunCommands
{
    public class Config : IConfig
    {

        public bool IsEnabled { get; set; } = true;

        public bool Debug { get; set; } = false;

        [Description("Cooldown hint text")]
        public string CooldownHintText = "Wait <color={rolecolor}>{time}</color> seconds before using this again.";

        [Description("If .push is enabled")]

        public bool PushEnabled { get; set; } = true;

        [Description("Range at which the pusher can push someone")]
        public float PushRange { get; set; } = 1.25f;



        [Description("Cooldown in seconds between each push")]
        public float PushCooldown { get; set; } = 2f;

        [Description("Push Force, how much they get pushed")]
        public float PushForce { get; set; } = 1.7f;


        [Description("More iterations = more smoother push at cost of performance")]
        public int Iterations { get; set; } = 15;


        [Description("Message showed to victim when pushed.")]
        public string PushHintVictim { get; set; } = "You have been pushed by <color=purple>{player}</color>!";


        [Description("Message showed to instigator when victim is pushed.")]
        public string PushHintInstigator { get; set; } = "You pushed <color={rolecolor}>{player}</color>!";



        [Description("If .pat is enabled")]
        public bool PatEnabled { get; set; } = true;
        [Description("Range at which the patter can pat someone")]
        public float PatRange { get; set; } = 2.5f;

        [Description("Cooldown in seconds between each pat")]
        public float PatCooldown { get; set; } = 10f;

        [Description("Can SCPS use .pat")]
        public bool PatCanSCPSPat { get; set; } = false;

        [Description("Can you .pat any role")]
        public bool PatAnyone { get; set; } = false;

        [Description("Allowed roles to be patted if the above is false")]
        public IEnumerable<RoleTypeId> PatableRoles { get; set; } = new List<RoleTypeId>() 
        {
            RoleTypeId.Scp939
        };
        [Description("Amount of health given to patted player")]
        public float PatHealthGrant { get; set; } = 2f;

        [Description("Whether the health granted can bypass maxhealth")]
        public bool PatHealthExceedMax { get; set; } = false;

        [Description("Message showed to victim when patted.")]
        public string PatHintVictim { get; set; } = "You have been patted by <color={rolecolor}>{player}</color>!\nGained <color=green>{hp}</color> health!";

        [Description("Message showed to instigator when victim is patted.")]
        public string PatHintInstigator { get; set; } = "You patted <color={rolecolor}>{player}</color> and gave them <color=green>{hp}</color> health!";

        [Description("Message shown when scps cannot use the pat command")]
        public string PatHintSCPCantUse { get; set; } = "<color=red>SCPS cannot use this command</color>";

        [Description("Message shown when you can't pat that role")]
        public string PatHintCantPatRole { get; set; } = "<color=red>Can't pat this role!</color>";

        [Description("If .punch is enabled")]

        public bool PunchEnabled { get; set; } = true;

        [Description("Range at which the attacker can punch someone")]
        public float PunchRange { get; set; } = 1.25f;

        [Description("Damage taken from the punch")]
        public float PunchDamage { get; set; } = 2f;

        [Description("Cooldown in seconds between each punch")]
        public float PunchCooldown { get; set; } = 2f;

        [Description("Push Force, how much they get pushed")]
        public float PunchForce { get; set; } = 1.7f;


        [Description("More iterations = more smoother push at cost of performance")]
        public int PunchIterations { get; set; } = 15;


        [Description("Message showed to victim when pushed.")]
        public string PunchHintVictim { get; set; } = "You have been punched by <color=purple>{player}</color>!";


        [Description("Message showed to instigator when victim is pushed.")]
        public string PunchHintInstigator { get; set; } = "You punched <color={rolecolor}>{player}</color>!";
    }
}