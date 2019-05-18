using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TerrariaApi.Server;
using Utils;

namespace TShock_Presence
{
    [ApiVersion(2, 1)]
    public class TShock_Presence : TerrariaPlugin
    {
        public override string Author
        {
            get { return "Yaekith"; }
        }
        public override string Description
        {
            get { return "Gives you a nice presence on discord telling people what server you're hosting, etc."; }
        }

        public override string Name
        {
            get { return "TShockPresence -- TShock Discord Rich Presence"; }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
        }

        public TShock_Presence(Main game) : base(game)
        {
            Order = 1;
        }

        public override void Initialize()
        {
            ServerApi.Hooks.ServerJoin.Register(this, OnJoin);
            ServerApi.Hooks.ServerLeave.Register(this, OnLeave);
            TShockDiscordPresence.Start();
            CheckForConfigFile();
            Console.WriteLine("TShockPresence has Loaded.");
        }

        private void CheckForConfigFile()
        {
            if (!File.Exists("TShockPresenceConfig.json"))
            {
                File.WriteAllText("TShockPresenceConfig.json", JsonConvert.SerializeObject(new Config { applicationID = "578495448273256450", LargeImageKey = "tshock_small_icon", SmallImageKey = "tshock_normal" }));
            }

            Utils.Configuration = JsonConvert.DeserializeObject<Config>(File.ReadAllText("TShockPresenceConfig.json"));
        }

        private void OnJoin(JoinEventArgs args)
        {
            Utils.PlayerCount++;
        }

        private void OnLeave(LeaveEventArgs args)
        {
            Utils.PlayerCount--;
        }
    }
}
