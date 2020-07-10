using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SMoDABot.Models.Commands
{
    public class HelloCommand : Command
    {
        public override string Name => @"/hello";

        public override async Task Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            await client.SendTextMessageAsync(chatId: chatId, text: "Hello" ,replyToMessageId: messageId);
        }
        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;

            return message.Text.Contains(this.Name);
        }
    }
}