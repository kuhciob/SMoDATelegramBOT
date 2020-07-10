using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.UI.WebControls;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SMoDABot.Models.Commands
{
    public class StartCommand : Command
    {
        public override string Name => @"/start";

        public override async Task Execute(Message message, TelegramBotClient client)
        {
            try
            {
                var chatId = message.Chat.Id;
                var messageId = message.MessageId;
                int step = 0;

                using (BOTdbEntities db = new BOTdbEntities())
                {
                    var chat = db.Chats.Find(chatId);
                    if (chat != null)
                    {
                        step = chat.AlgorithmStep;
                        await client.SendTextMessageAsync(chatId: chatId, text: chat.ChatId.ToString());
                    }
                    else
                    {
                        db.Chats.Add(new Chats { ChatId = chatId });
                        db.SaveChanges();
                    }
                }

                    var startMenu = new InlineKeyboardMarkup(
                    new[]
                    {
                        new[] {
                            new InlineKeyboardButton { Text = "DSR", CallbackData= "dsr" }
                        },
                        new[] {
                            new InlineKeyboardButton { Text = "Кореляція", CallbackData="correlation" }
                        },
                        new[] {
                            new InlineKeyboardButton { Text = "Регресія", CallbackData="regression" }
                        },
                        new[] {
                            new InlineKeyboardButton { Text = "Instagram автора", Url="https://www.instagram.com/lvasuk/" }
                        }
                    });

                await client.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Розв'язати",
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                    replyMarkup: startMenu
                   );
            }
            catch (Exception ex)
            {
                Exception exc = ex;
                string s = new System.Text.StringBuilder().AppendLine(ex.Message).AppendLine(exc.Source).AppendLine(exc.StackTrace).ToString();
                await client.SendTextMessageAsync(chatId: message.Chat.Id, text: s);
            }
        }
        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;

            return message.Text.Contains(this.Name);
        }
    }
}