using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DiscordRPCUtils;
using TShockAPI;

namespace Utils
{
    public static class TShockDiscordPresence
    {
        public static DiscordRPC.RichPresence Presence = new DiscordRPC.RichPresence();
        public static void Start()
        {
            DiscordRPC.EventHandlers eventHandler = default(DiscordRPC.EventHandlers);
            new Thread(() =>
            {
                DiscordRPC.Initialize("578495448273256450", ref eventHandler, false, null);
                for(; ;)
                {
                    Thread.Sleep(30000);
                    Update();
                }
            }) { IsBackground = true}.Start();
        }

        private static void Update()
        {
            Presence.details = "Hosting a Server";
            Presence.state = $"on {GetIP()}:{GetPort()}";
            Presence.largeImageKey = "tshock_small_icon";
            Presence.largeImageText = $"With {GetPlayers()} player(s)";
            Presence.smallImageKey = "tshock_normal";
            Presence.smallImageText = $"Using {GetPluginCount()} plugin(s)";
            DiscordRPC.UpdatePresence(Presence);
        }

        private static object GetPlayers()
        {
            return TShock_Presence.Utils.PlayerCount;
        }

        private static object GetPluginCount()
        {
            int fakePluginsCount = 0;
            foreach(string file in Directory.GetFiles("ServerPlugins"))
            {
                if (file.Contains("BCrypt.Net") || file.Contains("HttpServer") || file.Contains("Mono.Data.Sqlite") || file.Contains("MySql.Data") || file.Contains("TShockAPI"))
                {
                    fakePluginsCount++;
                }
            }
            return Directory.GetFiles("ServerPlugins").Count() - fakePluginsCount;
        }

        public static string GetIP()
        {
            if (Terraria.Netplay.ServerIPText == "127.0.0.1") { return "localhost"; }
            return new WebClient().DownloadString("https://api.ipify.org");
        }
        public static string GetPort()
        {
            return Terraria.Netplay.ListenPort.ToString();
        }
    }
}
