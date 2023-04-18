using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Discord_Bot
{
    public class ModSample : ModuleBase<SocketCommandContext>
    {
        [Command("kick")]
        [Summary("Kick a user from the server.")]
        [RequireBotPermission(GuildPermission.KickMembers)]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task Kick(SocketGuildUser targetUser, [Remainder]string reason = "No reason provided.")
        {
            await targetUser.KickAsync(reason);
            await ReplyAsync($"**{targetUser}** has been kicked. Bye bye :wave:");
        }

        [Command("ban")]
        [Summary("Ban a user from the server")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task Ban(SocketGuildUser targetUser, [Remainder]string reason = "No reason provided.")
        {
            await Context.Guild.AddBanAsync(targetUser.Id, 0, reason);
            await ReplyAsync($"**{targetUser}** has been banned. Bye bye :wave:");
        }

        [Command("unban")]
        [Summary("Unban a user from the server")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Unban(ulong targetUser)
        {
            await Context.Guild.RemoveBanAsync(targetUser);
            await Context.Channel.SendMessageAsync($"The user has been unbanned :clap:");
        }

        [Command("clear")]
        [Summary("Bulk deletes messages in chat")]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Purge(int delNumber)
        {
            var channel = Context.Channel as SocketTextChannel;
            var items = await channel.GetMessagesAsync(delNumber + 1).FlattenAsync();
            await channel.DeleteMessagesAsync(items);
        }

        [Command("reloadconfig")]
        [Summary("Reloads the config and applies changes")] 
        [RequireOwner] // Require the bot owner to execute the command successfully.
        public async Task ReloadConfig()
        {
            await Functions.SetBotStatusAsync(Context.Client);
            await ReplyAsync("Reloaded!");
        }
        [Command("mute")]
        [RequireUserPermission(GuildPermission.MuteMembers)]
        public async Task Mute(SocketGuildUser user, int duration, string reason)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var roleId = Context.Guild.GetRole(995422590342533180);
            await user.AddRoleAsync(roleId);

            await ReplyAsync($"User {user} has benn muted for {duration} sec. Reason: {reason}");

            Thread.Sleep(duration*1000);

            await user.RemoveRoleAsync(roleId);
            await ReplyAsync($"User {user} has been unmuted. Time is dead :)");
        }
        [Command("unmute")]
        [RequireUserPermission(GuildPermission.MuteMembers)]
        public async Task UnMute(SocketGuildUser user)
        {
            var roleId = Context.Guild.GetRole(995422590342533180);
            await user.RemoveRoleAsync(roleId);
            await ReplyAsync($"User {user} has been unmuted.");
        }
        [Command("help")]
        public async Task Help() {
            EmbedBuilder embed = new EmbedBuilder()
                .WithColor(255, 0, 225)
                .WithDescription("Command list:\n" +
                ".hello - Say hello to the bot.\n" +
                ".pick [choise] - Pick something.\n" +
                ".cookie - Give someone a cookie\n" +
                ".ping - Show current latency.\n" +
                ".avatar - Get a user's avatar.\n" +
                ".info - Show server information.\n" +
                ".role - Show information about a role.\n" +
                ".mute [ping user] [duration] [reason] - Mute user.\n" +
                ".unmute [ping user] - unmute user\n" +
                ".ban [ping user] [reason] - Ban a user from the server.\n" +
                ".unban [ping user] - Unban a user from the server.\n" +
                ".clear [amount] - Bulk deletes messages in chat");

            await ReplyAsync(embed: embed.Build());
        }
    }
}
