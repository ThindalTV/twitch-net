using System;
using System.Net;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwitchWrapper.Core;
using TwitchWrapper.Core.Commands;
using TwitchWrapper.Core.IrcClient;
using Xunit;

namespace TwitchWrapper.Test
{
    public class WebSocketClientTests
    {
        private const string OAUTH = "oauth:81ne20q96iai9dwbgfn4sjgvod8e6w";

        [Fact]
        public async Task IrcClient_AuthenticateSucessfully()
        {
            var tcs = new TaskCompletionSource<bool>();

            using (var client = new TwitchIrcClient("irc.twitch.tv", 6667))
            {
                client.SubscribeReceive += (command) =>
                {
                    var result = command.Parse();
                    Assert.Equal("Welcome, GLHF!", result);
                    tcs.TrySetResult(true);
                };

                await client.SendAsync(new AuthenticateCommand("thatnandotho", "oauth:gv42lzn9if3crb3o6ezpbxh53rv9ok"));

                await tcs.Task;
            }
        }

        [Fact]
        public async Task IrcClient_JoinChannel()
        {
            var tcs = new TaskCompletionSource<bool>();

            using (var client = new TwitchIrcClient("irc.twitch.tv", 6667))
            {
                client.SubscribeReceive += (command) =>
                {
                    var result = command.Parse();
                };

                client.StartReceive();
                await client.SendAsync(new AuthenticateCommand("thatnandotho", "oauth:gv42lzn9if3crb3o6ezpbxh53rv9ok"));
                await client.SendAsync(new JoinCommand("thatnandotho"));

                await tcs.Task;
            }
        }
    }
}