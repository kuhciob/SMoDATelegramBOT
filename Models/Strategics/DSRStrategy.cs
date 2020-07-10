using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;
using SMoDALib;
using Telegram.Bot.Types.InputFiles;
using System.Net;
using System.Drawing;
using Microsoft.Ajax.Utilities;
using System.Net.Http.Headers;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace SMoDABot.Models.Strategics
{
    public class DSRStrategy : Strategy
    {
        public override string Name => @"dsr";
        private Telegram.Bot.Types.Message caughtMessage;
        private bool flag = false;
        private long chatId;
        private List<double> Array = null;
        private TelegramBotClient clientBot;
        CallbackQuery callbackQuery;
        public override async Task Action(CallbackQuery callback, TelegramBotClient client)
        {
            try
            {
                clientBot = client;
                callbackQuery = callback;
                ForceReplyMarkup forceReplyMarkup = new ForceReplyMarkup();

                await client.SendTextMessageAsync(chatId: callback.Message.Chat.Id, text: "Введіть Дискретний Статистичний Ряд",replyMarkup:forceReplyMarkup );

                chatId = callback.Message.Chat.Id;

                await ChartBuilder.SendChart(callback, client);
                await client.AnswerCallbackQueryAsync(callbackQueryId: callback.Id, text: "OK");
            }
            catch (Exception ex)
            {
                Exception exc = ex;
                string s = new StringBuilder().AppendLine(ex.Message).AppendLine(exc.Source).AppendLine(exc.StackTrace).ToString();               
                await client.SendTextMessageAsync(chatId: callback.Message.Chat.Id, text: s);
            }
        }
        private async void CatchMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            caughtMessage = e.Message;
            await clientBot.SendTextMessageAsync(chatId: callbackQuery.Message.Chat.Id, text: "i am in");
            if (caughtMessage.Chat.Id == chatId)
            {
                Array = Tools.UserMessageConverter.ToNumericArray(caughtMessage.Text);
                if (Array == null)
                {
                    await clientBot.SendTextMessageAsync(chatId: callbackQuery.Message.Chat.Id, text: "Помилка при введені вибірки. Спробуйте ще раз");
                }
                else
                {
                    clientBot.StopReceiving();
                    clientBot.OnMessage -= CatchMessage;
                }
            }
        }
    }
}