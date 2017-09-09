using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace DSPlus.Examples
{
    public class ExampleUngrouppedCommands
    {
        [Command("ping")] // let's define this method as a command
        [Description("Example ping command")] // this will be displayed to tell users what this command does when they invoke help
        [Aliases("pong")] // alternative names for the command
        public async Task Ping(CommandContext ctx) // this command takes no arguments
        {
            // let's trigger a typing indicator to let
            // users know we're working
            await ctx.TriggerTypingAsync();

            // let's make the message a bit more colourful
            
                 var embed = new DiscordEmbed
                {
                    Title = "Pong!",
                    Description = $"This took {ctx.Client.Ping}ms to send!",
                };
                await ctx.RespondAsync("", embed: embed);
            // respond with ping
        }

      
        [Command("hello"), Description("Says hi to specified user."), Aliases("sayhi", "say_hi")]
        public async Task Greet(CommandContext ctx) // this command takes a member as an argument; you can pass one by username, nickname, id, or mention
        {
            // note the [Description] attribute on the argument.
            // this will appear when people invoke help for the
            // command.

            // let's trigger a typing indicator to let
            // users know we're working
            await ctx.TriggerTypingAsync();
                var helloembed = new DiscordEmbed
                {
                    Title = "WolfieCBOT",
                   Description = "Hello there!",
                   Color = 0x00ff11
                };
                await ctx.RespondAsync("", embed: helloembed);
            // and finally, let's respond and greet the user.
        }

        [Command("sum"), Description("Sums all given numbers and returns said sum.")]
        public async Task SumOfNumbers(CommandContext ctx, [Description("Integers to sum.")] params int[] args)
        {
            // note the params on the argument. It will indicate
            // that the command will capture all the remaining arguments
            // into a single array

            // let's trigger a typing indicator to let
            // users know we're working
            await ctx.TriggerTypingAsync();

            // calculate the sum
            var sum = args.Sum();
                var calcembed = new DiscordEmbed
                {
                    Title = "Calculator",
                   Description = $"The sum is {sum.ToString("#,##0")}",
                   Color = 0x00ff11
                };
                await ctx.RespondAsync("", embed: calcembed);
            // and send it to the user
            await ctx.RespondAsync($"The sum of these numbers is {sum.ToString("#,##0")}");
        }

        // this command will use our custom type, for which have 
        // registered a converter during initialization
        [Command("math"), Description("Does basic math.")]
        public async Task Math(CommandContext ctx, [Description("Operation to perform on the operands.")] MathOperation operation, [Description("First operand.")] double num1, [Description("Second operand.")] double num2)
        {
            var result = 0.0;
            switch (operation)
            {
                case MathOperation.Add:
                    result = num1 + num2;
                    break;

                case MathOperation.Subtract:
                    result = num1 - num2;
                    break;
                    
                case MathOperation.Multiply:
                    result = num1 * num2;
                    break;

                case MathOperation.Divide:
                    result = num1 / num2;
                    break;

                case MathOperation.Modulo:
                    result = num1 % num2;
                    break;
            }

                var calcembed = new DiscordEmbed
                {
                    Title = "Calculator",
                   Description = $"The result is {result.ToString("#,##0.00")}",
                   Color = 0x00ff11
                };
                await ctx.RespondAsync("", embed: calcembed);
        }
        [Command("invitebot"), Description("A invite link for the bot.")]
        public async Task SendInviteBotDM(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
              var checkdms = new DiscordEmbed
                {
                    Title = "Sent!",
                   Description = "Check your DMs!",
                   Color = 0x00ff11
                };
                await ctx.RespondAsync("", embed: checkdms);
                 var invite = new DiscordEmbed
                {
                
                   Title = "Invite me to your server!",
                   Description = "https://discordapp.com/oauth2/authorize?client_id=329658186237739008&scope=bot&permissions=469888143",
                   Color = 0x0084f9
                };
                await ctx.Member.SendMessageAsync("", embed: invite);
        }
        [Command("about"), Description("Tells you about WolfieCBOT")]
        public async Task TellUser(CommandContext ctx)
        {
            var embed = new DiscordEmbed
            {
               Title = "About WolfieCBOT",
                Description = "WolfieCBOT is a bot which started off with Discord.NET 0.9.6 which has moved on to DSharpPlus! Type $help for a list of commands!"
            };
            await ctx.RespondAsync("", embed: embed);
        }
        [Command("purge"), Description("Purges 50 messages"), RequirePermissions(Permissions.ManageMessages), Aliases("prune","clear")]
        public async Task PurgeChat(CommandContext ctx) 
        {
            try {
           var ids = (await ctx.Channel.GetMessagesAsync(before: ctx.Message.Id, limit: 50));
            await ctx.Channel.DeleteMessagesAsync(ids);
             var embed = new DiscordEmbed
                {
                    Title = $"Purged {ctx.Channel.Name}",
                   Description = $"{ids.Count} messages were deleted!",
                   Color = 0xFF0000 
                };
                await ctx.RespondAsync("", embed: embed);
 
            }
             catch (Exception)
            {
              
                 var embed = new DiscordEmbed
                {
                    Title = "Uh Oh!",
                   Description = "I can't purge! I don't have permission or the messages are older than 2 weeks!",
                   Color = 0xFF0000 
                };
                await ctx.RespondAsync("", embed: embed);
            }
        }
    }
    [Group("owner")] // let's mark this class as a command group
    [Description("Owner commands."), Hidden, RequireOwner]
    public class OwnerGrouppedCommands
    {
        [Command("setgame"), Description("Changes the game."), RequireOwner]
        public async Task ChangeYourGame(CommandContext ctx, [RemainingText]string newgame)
        {
            Console.WriteLine($"[Info] Changing game to {newgame}");
            await ctx.Client.UpdateStatusAsync(game:new Game(newgame) { StreamType = GameStreamType.NoStream});
            var embed = new DiscordEmbed
            {
                Title = "Success",
                Description = $"The game has been changed to {newgame}.",
                Color = 0x00ff11
            };
            await ctx.RespondAsync("", embed: embed);
            return;
        }
     [Command("hi"), Description("Says hi to owner"), RequireOwner]
     public async Task SayHiToOwner(CommandContext ctx)
     {
        await ctx.Channel.SendMessageAsync("Hello Wolfie!");
     }
     [Command("purge"), Description("Purges 50 messages"), RequireOwner, Aliases("prune","clear")]
        public async Task PurgeChat(CommandContext ctx) 
        {
            try {
           var ids = (await ctx.Channel.GetMessagesAsync(before: ctx.Message.Id, limit: 50));
            await ctx.Channel.DeleteMessagesAsync(ids);
             var embed = new DiscordEmbed
                {
                    Title = $"Purged {ctx.Channel.Name}",
                   Description = $"{ids.Count} messages were deleted!",
                   Color = 0xFF0000 
                };
                await ctx.RespondAsync("", embed: embed);
 
            }
             catch (Exception)
            {
              
                 var embed = new DiscordEmbed
                {
                    Title = "Uh Oh!",
                   Description = "I can't purge! I don't have permission or the messages are older than 2 weeks!",
                   Color = 0xFF0000 
                };
                await ctx.RespondAsync("", embed: embed);
            }
        }
      [Command("nick"), Description("Gives someone a new nickname."), RequireOwner]
        public async Task ChangeNickname(CommandContext ctx, [Description("Member to change the nickname for.")] DiscordMember member, [RemainingText, Description("The nickname to give to that user.")] string new_nickname)
        {
            // let's trigger a typing indicator to let
            // users know we're working
            await ctx.TriggerTypingAsync();

            try
            {
                // let's change the nickname, and tell the 
                // audit logs who did it.
                await member.ModifyAsync(new_nickname, reason: $"Changed by {ctx.User.Username} ({ctx.User.Id}).");
                
                // let's make a simple response.
               var embed = new DiscordEmbed
                {
                    Title = "Success",
                   Description = "You changed the users nickname!",
                   Color = 0x00ff11
                };
                await ctx.RespondAsync("", embed: embed);
            }
            catch (Exception)
            {
                // oh no, something failed, let the invoker now
                 var embed = new DiscordEmbed
                {
                    Title = "Uh Oh!",
                   Description = "I can't nick that user! I don't have permission.",
                   Color = 0xFF0000 
                };
                await ctx.RespondAsync("", embed: embed);
            }
        }
        [Command("kick"), Description("Kicks a member."), RequireOwner]
        public async Task KickUser(CommandContext ctx, [Description("Member to kick.")] DiscordMember member)
        {
            // let's trigger a typing indicator to let
            // users know we're working
            await ctx.TriggerTypingAsync();

            try
            {
                // let's change the nickname, and tell the 
                // audit logs who did it.
                await ctx.Guild.RemoveMemberAsync(member);
                
                // let's make a simple response.
               var embed = new DiscordEmbed
                {
                    Title = "Success",
                   Description = "You kicked the user!",
                   Color = 0x00ff11
                };
                await ctx.RespondAsync("", embed: embed);
            }
            catch (Exception)
            {
                // oh no, something failed, let the invoker now
                 var embed = new DiscordEmbed
                {
                    Title = "Uh Oh!",
                   Description = "I can't kick that user! I don't have permission.",
                   Color = 0xFF0000 
                };
                await ctx.RespondAsync("", embed: embed);
            }
        }
        
        [Command("ban"), Description("Bans a member."), RequireOwner]
        public async Task BanUser(CommandContext ctx, [Description("Member to ban.")] DiscordMember member)
        {
            // let's trigger a typing indicator to let
            // users know we're working
            await ctx.TriggerTypingAsync();

            try
            {
                // let's change the nickname, and tell the 
                // audit logs who did it.
                await ctx.Guild.BanMemberAsync(member);
                
                // let's make a simple response.
               var embed = new DiscordEmbed
                {
                    Title = "Success",
                   Description = $"You banned {member}!",
                   Color = 0x00ff11
                };
                await ctx.RespondAsync("", embed: embed);
            }
            catch (Exception)
            {
                // oh no, something failed, let the invoker now
                 var embed = new DiscordEmbed
                {
                    Title = "Uh Oh!",
                   Description = "I can't ban that user! I don't have permission.",
                   Color = 0xFF0000 
                };
                await ctx.RespondAsync("", embed: embed);
            }
        }
        [Command("unban"), Description("Unban a member."), RequireOwner]
        public async Task UnbanUser(CommandContext ctx, [Description("Member to unban.")] DiscordMember member)
        {
            // let's trigger a typing indicator to let
            // users know we're working
            await ctx.TriggerTypingAsync();

            try
            {
                // let's change the nickname, and tell the 
                // audit logs who did it.
                await ctx.Guild.UnbanMemberAsync(member);
                
                // let's make a simple response.
               var embed = new DiscordEmbed
                {
                    Title = "Success",
                   Description = $"You unbanned {member}!",
                   Color = 0x00ff11
                };
                await ctx.RespondAsync("", embed: embed);
            }
            catch (Exception)
            {
                // oh no, something failed, let the invoker now
                 var embed = new DiscordEmbed
                {
                    Title = "Uh Oh!",
                   Description = "I can't unban that user! I don't have permission.",
                   Color = 0xFF0000 
                };
                await ctx.RespondAsync("", embed: embed);
            }
        }
        [Command("createrole"), Description("Creates a role."), RequireOwner]
        
        public async Task CreateRole(CommandContext ctx, [Description("Role parameter")] DiscordRole role)
        {
            // let's trigger a typing indicator to let
            // users know we're working
            await ctx.TriggerTypingAsync();

            try
            {
                // let's change the nickname, and tell the 
                // audit logs who did it.
                string rolename = ctx.RawArgumentString;
                await ctx.Guild.CreateRoleAsync(rolename);
                
                // let's make a simple response.
               var embed = new DiscordEmbed
                {
                    Title = "Success",
                   Description = $"You created role {rolename}!",
                   Color = 0x00ff11
                };
                await ctx.RespondAsync("", embed: embed);
            }
            catch (Exception)
            {
                // oh no, something failed, let the invoker now
                 var embed = new DiscordEmbed
                {
                    Title = "Uh Oh!",
                   Description = "I can't make that role! I don't have permission.",
                   Color = 0xFF0000 
                };
                await ctx.RespondAsync("", embed: embed);
            }
        }
        [Command("mute"), Description("Mutes a person."), RequireOwner]
        
        public async Task MutePerson(CommandContext ctx, [Description("Role parameter")] DiscordMember member)
        {
           
            await ctx.TriggerTypingAsync();

            try
            {
                
                var Muted = ctx.Guild.GetRole(333642323269255178);
                await member.GrantRoleAsync(Muted);




                var embed = new DiscordEmbed
                {
                    Title = "Success",
                    Description = $"You muted {member}!",
                    Color = 0x00ff11
                };
                await ctx.RespondAsync("", embed: embed);
            }
            catch (Exception)
            {
              
                 var embed = new DiscordEmbed
                {
                    Title = "Uh Oh!",
                   Description = "I can't mute that person! I don't have permission or the role is not correct!",
                   Color = 0xFF0000 
                };
                await ctx.RespondAsync("", embed: embed);
            }
        }
        [Command("unmute"), Description("Unmutes a person."), RequireOwner]
        
        public async Task UnmutePerson(CommandContext ctx, [Description("Role parameter")] DiscordMember member, DiscordRole Name)
        {
           
            await ctx.TriggerTypingAsync();

            try
            {
                
                var Muted = ctx.Guild.GetRole(333643182338473986);
                await member.TakeRoleAsync(Muted);




                var embed = new DiscordEmbed
                {
                    Title = "Success",
                    Description = $"You unmuted {member}!",
                    Color = 0x00ff11
                };
                await ctx.RespondAsync("", embed: embed);
            }
            catch (Exception)
            {
              
                 var embed = new DiscordEmbed
                {
                    Title = "Uh Oh!",
                   Description = "I can't unmute that person! I don't have permission or the role is not correct!",
                   Color = 0xFF0000 
                };
                await ctx.RespondAsync("", embed: embed);
            }
        }
    }
    [Group("admin")] // let's mark this class as a command group
    [Description("Administrative commands.")] // give it a description for help purposes
     // let's hide this from the eyes of curious users
    // and restrict this to users who have appropriate permissions
    public class ExampleGrouppedCommands
    {
        

        // all the commands will need to be executed as <prefix>admin <command> <arguments>

        // this command will be only executable by the bot's owner


        [Command("nick"), Description("Gives someone a new nickname."), RequirePermissions(Permissions.ManageNicknames)]
        public async Task ChangeNickname(CommandContext ctx, [Description("Member to change the nickname for.")] DiscordMember member, [RemainingText, Description("The nickname to give to that user.")] string new_nickname)
        {
            // let's trigger a typing indicator to let
            // users know we're working
            await ctx.TriggerTypingAsync();

            try
            {
                // let's change the nickname, and tell the 
                // audit logs who did it.
                await member.ModifyAsync(new_nickname, reason: $"Changed by {ctx.User.Username} ({ctx.User.Id}).");
                
                // let's make a simple response.
               var embed = new DiscordEmbed
                {
                    Title = "Success",
                   Description = "You changed the users nickname!",
                   Color = 0x00ff11
                };
                await ctx.RespondAsync("", embed: embed);
            }
            catch (Exception)
            {
                // oh no, something failed, let the invoker now
                 var embed = new DiscordEmbed
                {
                    Title = "Uh Oh!",
                   Description = "I can't nick that user! I don't have permission.",
                   Color = 0xFF0000 
                };
                await ctx.RespondAsync("", embed: embed);
            }
        }
        [Command("kick"), Description("Kicks a member."), RequirePermissions(Permissions.KickMembers)]
        public async Task KickUser(CommandContext ctx, [Description("Member to kick.")] DiscordMember member)
        {
            // let's trigger a typing indicator to let
            // users know we're working
            await ctx.TriggerTypingAsync();

            try
            {
                // let's change the nickname, and tell the 
                // audit logs who did it.
                await ctx.Guild.RemoveMemberAsync(member);
                
                // let's make a simple response.
               var embed = new DiscordEmbed
                {
                    Title = "Success",
                   Description = "You kicked the user!",
                   Color = 0x00ff11
                };
                await ctx.RespondAsync("", embed: embed);
            }
            catch (Exception)
            {
                // oh no, something failed, let the invoker now
                 var embed = new DiscordEmbed
                {
                    Title = "Uh Oh!",
                   Description = "I can't kick that user! I don't have permission.",
                   Color = 0xFF0000 
                };
                await ctx.RespondAsync("", embed: embed);
            }
        }
        
        [Command("ban"), Description("Bans a member."), RequirePermissions(Permissions.BanMembers)]
        public async Task BanUser(CommandContext ctx, [Description("Member to ban.")] DiscordMember member)
        {
            // let's trigger a typing indicator to let
            // users know we're working
            await ctx.TriggerTypingAsync();

            try
            {
                // let's change the nickname, and tell the 
                // audit logs who did it.
                await ctx.Guild.BanMemberAsync(member);
                
                // let's make a simple response.
               var embed = new DiscordEmbed
                {
                    Title = "Success",
                   Description = $"You banned {member}!",
                   Color = 0x00ff11
                };
                await ctx.RespondAsync("", embed: embed);
            }
            catch (Exception)
            {
                // oh no, something failed, let the invoker now
                 var embed = new DiscordEmbed
                {
                    Title = "Uh Oh!",
                   Description = "I can't ban that user! I don't have permission.",
                   Color = 0xFF0000 
                };
                await ctx.RespondAsync("", embed: embed);
            }
        }
        [Command("unban"), Description("Unban a member."), RequirePermissions(Permissions.BanMembers)]
        public async Task UnbanUser(CommandContext ctx, [Description("Member to unban.")] DiscordMember member)
        {
            // let's trigger a typing indicator to let
            // users know we're working
            await ctx.TriggerTypingAsync();

            try
            {
                // let's change the nickname, and tell the 
                // audit logs who did it.
                await ctx.Guild.UnbanMemberAsync(member);
                
                // let's make a simple response.
               var embed = new DiscordEmbed
                {
                    Title = "Success",
                   Description = $"You unbanned {member}!",
                   Color = 0x00ff11
                };
                await ctx.RespondAsync("", embed: embed);
            }
            catch (Exception)
            {
                // oh no, something failed, let the invoker now
                 var embed = new DiscordEmbed
                {
                    Title = "Uh Oh!",
                   Description = "I can't unban that user! I don't have permission.",
                   Color = 0xFF0000 
                };
                await ctx.RespondAsync("", embed: embed);
            }
        }
        [Command("createrole"), Description("Creates a role."), RequirePermissions(Permissions.ManageRoles)]
        
        public async Task CreateRole(CommandContext ctx, [Description("Role parameter")] DiscordRole role)
        {
            // let's trigger a typing indicator to let
            // users know we're working
            await ctx.TriggerTypingAsync();

            try
            {
                // let's change the nickname, and tell the 
                // audit logs who did it.
                string rolename = ctx.RawArgumentString;
                await ctx.Guild.CreateRoleAsync(rolename);
                
                // let's make a simple response.
               var embed = new DiscordEmbed
                {
                    Title = "Success",
                   Description = $"You created role {rolename}!",
                   Color = 0x00ff11
                };
                await ctx.RespondAsync("", embed: embed);
            }
            catch (Exception)
            {
                // oh no, something failed, let the invoker now
                 var embed = new DiscordEmbed
                {
                    Title = "Uh Oh!",
                   Description = "I can't make that role! I don't have permission.",
                   Color = 0xFF0000 
                };
                await ctx.RespondAsync("", embed: embed);
            }
        }
        [Command("mute"), Description("Mutes a person."), RequirePermissions(Permissions.ManageMessages)]
        
        public async Task MutePerson(CommandContext ctx, [Description("Role parameter")] DiscordMember member)
        {
           
            await ctx.TriggerTypingAsync();

            try
            {
                
                var Muted = ctx.Guild.GetRole(333642323269255178);
                await member.GrantRoleAsync(Muted);




                var embed = new DiscordEmbed
                {
                    Title = "Success",
                    Description = $"You muted {member}!",
                    Color = 0x00ff11
                };
                await ctx.RespondAsync("", embed: embed);
            }
            catch (Exception)
            {
              
                 var embed = new DiscordEmbed
                {
                    Title = "Uh Oh!",
                   Description = "I can't mute that person! I don't have permission or the role is not correct!",
                   Color = 0xFF0000 
                };
                await ctx.RespondAsync("", embed: embed);
            }
        }
        [Command("unmute"), Description("Unmutes a person."), RequirePermissions(Permissions.ManageMessages)]
        
        public async Task UnmutePerson(CommandContext ctx, [Description("Role parameter")] DiscordMember member, DiscordRole Name)
        {
           
            await ctx.TriggerTypingAsync();

            try
            {
                
                var Muted = ctx.Guild.GetRole(333643182338473986);
                await member.TakeRoleAsync(Muted);




                var embed = new DiscordEmbed
                {
                    Title = "Success",
                    Description = $"You unmuted {member}!",
                    Color = 0x00ff11
                };
                await ctx.RespondAsync("", embed: embed);
            }
            catch (Exception)
            {
              
                 var embed = new DiscordEmbed
                {
                    Title = "Uh Oh!",
                   Description = "I can't unmute that person! I don't have permission or the role is not correct!",
                   Color = 0xFF0000 
                };
                await ctx.RespondAsync("", embed: embed);
            }
        }
        
    }
}
    
