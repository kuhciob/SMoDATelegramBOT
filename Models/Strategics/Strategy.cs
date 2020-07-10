using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SMoDABot.Models.Strategics
{
    public abstract class Strategy
    {
        public abstract string Name { get; }
        public abstract Task Action(CallbackQuery callback, TelegramBotClient client);

    }
}