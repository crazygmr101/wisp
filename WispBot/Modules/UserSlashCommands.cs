using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace WispBot.Modules
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public partial class SlashModule : SlashCommandModule
    {
        [SlashCommandGroup("user", "Get user information")]
        public class UserInfoGroup : SlashCommandModule
        {
            [SlashCommand("avatar", "Show a user's avatar")]
            public async Task Av(
                InteractionContext ctx,
                [Option("user", "The user to get it for")]
                DiscordUser user = null
            )
            {
                var member = user is null
                    ? await ctx.Guild.GetMemberAsync(ctx.Member.Id)
                    : await ctx.Guild.GetMemberAsync(user.Id);
                var embed = new DiscordEmbedBuilder
                {
                    Title = $"{member.Username}'s Avatar",
                    ImageUrl = member.AvatarUrl
                };
                var builder = new DiscordInteractionResponseBuilder().AddEmbed(embed.Build());
                builder.IsEphemeral = user is null;
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    builder);
            }

            [SlashCommand("info", "Gets info on a user")]
            public async Task Info(
                InteractionContext ctx,
                [Option("user", "The user to get it for")]
                DiscordUser user = null
            )
            {
                var member = user is null
                    ? await ctx.Guild.GetMemberAsync(ctx.Member.Id)
                    : await ctx.Guild.GetMemberAsync(user.Id);
                var embed = new DiscordEmbedBuilder
                {
                    Title = $"{member.Username}",
                }.WithThumbnail(member.AvatarUrl);

                if (member.Nickname is not null)
                    embed.AddField("Nickname", member.Nickname);

                embed.AddField("ID", member.Id.ToString());
                embed.AddField("Joined Discord", member.JoinedAt.ToString());
                embed.AddField($"Joined {ctx.Guild.Name}", member.CreationTimestamp.ToString());

                var hoistedRoles = member.Roles.Where(role => role.IsHoisted).ToArray();
                embed.AddField($"Hoisted Roles ({hoistedRoles.Length})", string.Join("", hoistedRoles.Take(6).Select(role => role.Mention)), true);
                
                var normalRoles = member.Roles.Where(role => !role.IsHoisted).Where(role => role.Id != ctx.Guild.Id).ToArray();
                embed.AddField($"Normal Roles ({normalRoles.Length})", string.Join("", normalRoles.Take(6).Select(role => role.Mention)), true);

                embed.AddField("Top Role", member.Roles.First().Mention, true);

                var builder = new DiscordInteractionResponseBuilder().AddEmbed(embed.Build());
                builder.IsEphemeral = user is null;
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    builder);
            }
        }
    }
}