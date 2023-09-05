using Exiled.API.Features;
using HarmonyLib;
using System;

namespace FunCommands
{
    public sealed class Plugin : Plugin<Config>
    {
        public override string Author => "BruteForceMaestro/Nutmaster(.push), morgana";

        public override string Name => "Roleplay Commands";

        public override string Prefix => Name;

        public static Plugin Instance;

        private EventHandlers _handlers;
        private Harmony _harmony;
        private string _harmornyid = $"morgana69698.rpcmd";
        public override void OnEnabled()
        {
            Instance = this;

            RegisterEvents();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnregisterEvents();

            Instance = null;

            base.OnDisabled();
        }

        private void RegisterEvents()
        {
            _handlers = new EventHandlers();
            Exiled.Events.Handlers.Server.WaitingForPlayers += _handlers.WaitingForPlayers;
            _harmony = new Harmony(_harmornyid);
            _harmony.PatchAll();
        }

        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= _handlers.WaitingForPlayers;
            _handlers = null;
            _harmony.UnpatchAll();
            _harmony = null;
        }
    }
}