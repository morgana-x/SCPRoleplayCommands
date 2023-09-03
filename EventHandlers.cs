using Exiled.Events.EventArgs;

namespace FunCommands
{
    internal class EventHandlers
    {
        public void WaitingForPlayers()
        {
            Push.Cooldowns.Clear();
            Pat.Cooldowns.Clear();
            Punch.Cooldowns.Clear();
        }
    }
}