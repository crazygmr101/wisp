using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

public class MessageSlashModule : SlashCommandModule
{
    [SlashCommandGroup("messages", "Work with messages")]
    public class MessagesCommandsGroup
    {
        [SlashCommand("clear", "Clear messages")]
        public async Task MessagesClear(
            InteractionContext ctx,
            [Option("num", "Number of messages to clear, defaults to 100")]
            long num = 100
        )
        {
            // TODO
        }
    }

    [SlashCommandGroup("message", "Send a message")]
    public class MessageGroup
    {
        [SlashCommand("send", "Send a plain text message")]
        public async Task MessageSend(
            InteractionContext ctx,
            [Option("content", "The message to send")]
            string content
        )
        {
            if (((await ctx.Guild.GetMemberAsync(ctx.Client.CurrentUser.Id)).PermissionsIn(ctx.Channel) &
                 Permissions.SendMessages) == 0)
            {
                await ctx.CreateResponseAsync(
                    InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder()
                        .AsEphemeral(true)
                        .WithContent("I can't send messages in that channel")
                );
                return;
            }

            await ctx.Channel.SendMessageAsync(content);

            await ctx.CreateResponseAsync(
                InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                    .AsEphemeral(true)
                    .WithContent("Sent!"));
        }

        //[SlashCommand("embed", "Embed a message")]
        public async Task MessageEmbed(
            InteractionContext ctx,
            [Option("Content", "The plain text content of the embed")]
            string content = null,
            [Option("Title", "The title of the embed")]
            string title = null,
            [Option("Description", "The description of the embed")]
            string description = null,
            [Option("Thumbnail", "The thumbnail of the embed")]
            string thumbnail = null,
            [Option("Image", "The image on the bottom of the embed")]
            string image = null)
        {
            if (((await ctx.Guild.GetMemberAsync(ctx.Client.CurrentUser.Id)).PermissionsIn(ctx.Channel) &
                 Permissions.SendMessages) == 0)
            {
                await ctx.CreateResponseAsync(
                    InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder()
                        .AsEphemeral(true)
                        .WithContent("I can't send messages in that channel")
                );
                return;
            }

            var embedBuilder = new DiscordEmbedBuilder();

            if (title is not null)
                embedBuilder.WithTitle(title);
            if (description is not null)
                embedBuilder.WithDescription(description);
            if (thumbnail is not null)
                embedBuilder.WithThumbnail(thumbnail);
            if (image is not null)
                embedBuilder.WithImageUrl(image);

            if (content is not null)
                await ctx.Channel.SendMessageAsync(content, embedBuilder.Build());
            else
                await ctx.Channel.SendMessageAsync(embedBuilder.Build());

            await ctx.CreateResponseAsync(
                InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                    .AsEphemeral(true)
                    .WithContent("Sent!"));
        }
    }
}