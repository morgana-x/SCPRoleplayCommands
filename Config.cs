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

        [Description("Cuffed hint text")]
        public string CuffedHintText = "<color=red>Can't use this while cuffed!</color>";

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
        public string PushHintVictim { get; set; } = "You have been pushed by <color={rolecolor}>{player}</color>!";


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
        public string PunchHintVictim { get; set; } = "You have been punched by <color={rolecolor}>{player}</color>!";


        [Description("Message showed to instigator when victim is pushed.")]
        public string PunchHintInstigator { get; set; } = "You punched <color={rolecolor}>{player}</color>!";

        [Description("Death message when death from punching occurs")]
        public string PunchDeathMessage { get; set; } = "Punched by {attacker}";


        [Description("Steal enabled")]
        public bool StealEnabled { get; set; } = true;

        [Description("Cooldown for stealing")]
        public float StealCooldown { get; set; } = 20f;

        [Description("Stealing chance")]
        public float StealChance { get; set; } = 0.25f;

        [Description("Stealing range")]
        public float StealRange { get; set; } = 1.25f;

        [Description("Steal Hint duration")]
        public float StealHintDuration { get; set; } = 5f;

        [Description("Hint shown to instigator when there is nothing in the victim's inventory")]
        public string StealEmptyInventoryHint { get; set; } = "<color={rolecolor}>{player}</color> has no items in their inventory!";

        [Description("Hint shown to victim when stealing fails")]
        public string StealFailHintVictim { get; set; } = "<color={rolecolor}>{player}</color> attempted to steal from you!";

        [Description("Hint shown to Instigator when stealing fails")]
        public string StealFailHintInstigator { get; set; } = "You failed to steal from <color={rolecolor}>{player}</color> they know. Run!";

        [Description("Hint shown to victim when stealing succeeds (LEAVE BLANK IF YOU DONT WANT THEM TO KNOW)")]
        public string StealSuccessHintVictim { get; set; } = "<color={rolecolor}>{player}</color> <color=red>stole</color> <color=green>{item}</color> from you!";

        [Description("Hint shown to Instigator when stealing succeeds")]
        public string StealSuccessHintInstigator { get; set; } = "<color=red>Stole</color>  <color=green>{item}</color> from <color={rolecolor}>{player}</color>!";

        [Description("Is Cuff enabled (Cuffing teammates)")]
        public bool CuffEnabled { get; set; } = true;

        [Description("Cuffed hint duration")]
        public float CuffHintDuration { get; set; } = 5f;

        [Description("Can you only use .cuff on teammates")]
        public bool CuffSameTeamOnly { get; set; } = true;

        [Description("Error hint shown when they are on different teams")]
        public string CuffSameTeamOnlyHint { get; set; } = "<color=red>You can only use .cuff on the same team!</color>";

        [Description("Do you have to be holding a firearm in order to cuff")]
        public bool CuffRequireFirearm { get; set; } = true;

        [Description("Error hint shown when they need to be holding a firearm")]
        public string CuffRequireFirearmHint { get; set; } = "<color=red>You need to be holding a firearm to do this!</color>";

        [Description("Cuffing cooldown")]
        public float CuffCooldown { get; set; } = 10f;

        [Description("Range at which you can cuff at")]
        public float CuffRange { get; set; } = 1.25f;

        [Description("Hint shown to Instigator at instance of cuffing")]
        public string CuffHintInstigator { get; set; } = "You have cuffed <color={rolecolor}>{player}</color>!";

        [Description("Hint shown to Instigator when they are already cuffed")]
        public string CuffHintAlreadyCuffed { get; set; } = "<color={rolecolor}>{player}</color> is already cuffed!";

    }
}