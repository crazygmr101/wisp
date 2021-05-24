using System.Runtime.CompilerServices;
using DSharpPlus.Entities;

namespace WispBot.Extensions
{
    public static class GuildExtensions
    {
        public static string IconUrlAs(this DiscordGuild guild, string extension)
        {
            return guild.IconUrl.Replace(guild.IconUrl.EndsWith(".jpg") ? ".jpg" : ".gif", $".{extension}");
        }

        public static string IconUrlGifPng(this DiscordGuild guild)
        {
            return guild.IconUrl.EndsWith(".gif") ? guild.IconUrl : guild.IconUrlAs("png");
        }
    }
}