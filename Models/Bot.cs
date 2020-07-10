using SMoDABot.Models.Commands;
using SMoDABot.Models.Strategics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;


namespace SMoDABot.Models
{
    public static class Bot
    {
        private static TelegramBotClient client;

        private static Dictionary<string, Command> commandsDictionary;
        private static Dictionary<string,Strategy> strategyDictionary;
        public static IReadOnlyDictionary<string, Strategy> Strategics { get => strategyDictionary; }
        public static IReadOnlyDictionary<string,Command> Commands { get => commandsDictionary; }
        public static async Task<TelegramBotClient> GetBotClientAsync()   
        {
            if(client != null)
            {
                return client;
            }
            else
            {
                commandsDictionary = new Dictionary<string, Command>();
                commandsDictionary.Add("/hello",new HelloCommand());
                commandsDictionary.Add("/start",new StartCommand());

                //await client.SetMyCommandsAsync(new BotCommand[] {
                //new BotCommand(){ Command="echo",Description="Is bot alive ??"},
                //}); 
                strategyDictionary = new Dictionary<string, Strategy>();
                strategyDictionary.Add("dsr", new DSRStrategy());
                //TODO: ADD MORE COMMANDs

                client = new TelegramBotClient(AppSettings.Key);
                var hook = string.Format(AppSettings.Url, "api/message/update");
                await client.SetWebhookAsync(hook);
                return client;
            }
        }

    }
}