using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using WispBot.Extensions;

namespace WispBot.Modules
{
    public class ServerSlashModule : SlashCommandModule
    {
        [SlashCommandGroup("server", "Commands to manage your server")]
        public class ServerGroup : SlashCommandModule
        {
            [SlashCommandGroup("icon", "Manage the server icon")]
            public class ServerIconSubgroup : SlashCommandModule
            {
                [SlashCommand("get", "Get the server icon")]
                public async Task ServerIconGet(InteractionContext ctx)
                {
                    await ctx.CreateResponseAsync(
                        InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder()
                            .AddEmbed(new DiscordEmbedBuilder()
                                .WithDescription(ctx.Guild.IconUrlGifPng())
                                .WithImageUrl(ctx.Guild.IconUrlGifPng()))
                    );
                }

                [SlashCommand("set", "Set the server icon")]
                public async Task ServerIconSet(
                    InteractionContext ctx,
                    [Option("url", "Url of the icon")] string url
                )
                {
                    if ((ctx.Member.PermissionsIn(ctx.Channel) & Permissions.ManageGuild) == 0)
                    {
                        await ctx.CreateResponseAsync(
                            InteractionResponseType.ChannelMessageWithSource,
                            new DiscordInteractionResponseBuilder()
                                .WithContent("You must have manage server permissions to run this command")
                                .AsEphemeral(true)
                        );
                        return;
                    }

                    await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                    var request = WebRequest.Create(url);

                    using (var response = await request.GetResponseAsync())
                    {
                        using (var ms = new MemoryStream())
                        {
                            var respStream = response.GetResponseStream();
                            if (respStream is null)
                            {
                                await ctx.CreateResponseAsync(
                                    InteractionResponseType.ChannelMessageWithSource,
                                    new DiscordInteractionResponseBuilder()
                                        .WithContent("An error occured while setting that image")
                                        .AsEphemeral(true)
                                );
                                return;
                            }

                            await respStream.CopyToAsync(ms);
                            ms.Position = 0;
                            await ctx.Guild.ModifyAsync(model => model.Icon = ms);
                        }
                    }


                    await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                        .AddEmbed(new DiscordEmbedBuilder()
                            .WithImageUrl(url)
                            .WithTitle("Icon set")));
                }
            }
        }
    }
}