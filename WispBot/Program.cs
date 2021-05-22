﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using WispBot.Modules;


namespace WispBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Out.WriteLine(Environment.GetEnvironmentVariables());
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            var discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = Environment.GetEnvironmentVariable("WISP_TOKEN"),
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.All
            });
            var slash = discord.UseSlashCommands();
            var guild = Environment.GetEnvironmentVariable("WISP_GUILD");

            if (guild is null)
                slash.RegisterCommands<SlashModule>();
            else
                slash.RegisterCommands<SlashModule>(ulong.Parse(guild));


            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}