using SMoDABot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Telegram.Bot.Types;


namespace SMoDABot.Controllers
{
    [Route(@"api/message/update")]
    public class MessageController : ApiController
    {

        [HttpGet]
        public string Get()
        {
            return "Method GET unuvalable";
        }

        [HttpPost]
        public async Task<OkResult> Post([FromBody] Update update)
        {
            if (update == null) return Ok();
            
            var botClient = await Bot.GetBotClientAsync();
            var commands = Bot.Commands;

            var message = update.Message;
            if (message != null && message?.Text != null) 
            {
                long chatId = message.Chat.Id;
                int step = 0;
                try                   
                {
                    string text = message.Text;
                    if (text[0] == '/')
                        await Bot.Commands[text].Execute(message, botClient);
                    else
                    {
                        using (BOTdbEntities db = new BOTdbEntities())
                        {
                            var chat = db.Chats.Find(chatId);
                            if (chat != null)
                            {
                                step = chat.AlgorithmStep;
                            }
                            else
                            {
                                db.Chats.Add(new Chats { ChatId = chatId });
                            }
                        }
                    }
                }
                catch (KeyNotFoundException)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Command not found", replyToMessageId: message.MessageId);
                }
                
                
            }           

            var callback = update.CallbackQuery;
            if (callback != null)
            {
                var chatId = callback.Message.Chat.Id;
                try
                {
                    await Bot.Strategics[callback.Data].Action(callback,botClient);                   
                }
                catch(KeyNotFoundException)
                {
                    await botClient.AnswerCallbackQueryAsync(callbackQueryId: callback.Id,text: "Error: callback don`t handled;",showAlert:true);
                }               
            }          
            return Ok();        
        }
    }
}
