using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot
{
    class Poll
    {
        public string PollId { get; set; }
        public Chat ChatId { get; set; }
    }
    public class TelegramClient
    {
        static ITelegramBotClient botClient;
        List<Poll> Polls = new List<Poll>();
        public void StartClient()
        {
            botClient = new TelegramBotClient("1452921547:AAE31ilXZ7sykFkOHlCC84FDITD6RDaNib0");

            var me = botClient.GetMeAsync().Result;
            botClient.OnUpdate += BotClient_OnUpdate;
            botClient.OnMessage += BotClient_OnMessage;
            botClient.StartReceiving();

        }

        private async void BotClient_OnUpdate(object sender, Telegram.Bot.Args.UpdateEventArgs e)
        {
            if (e.Update.PollAnswer != null)
            {
                //await botClient.SendTextMessageAsync(chatId: Polls.FirstOrDefault(x=>x.PollId == e.Update.PollAnswer.PollId).ChatId, text:e.Update.PollAnswer.OptionIds);
                var chatId = Polls.FirstOrDefault(x => x.PollId == e.Update.PollAnswer.PollId);
                if (e.Update.PollAnswer.OptionIds.Length != 0 && chatId != null)
                {
                    if (e.Update.PollAnswer.OptionIds[0] == 0)
                    {
                        List<string> answers = new List<string>();
                        answers.Add("Bye");
                        answers.Add("I`m Bad !");
                        var id = await botClient.SendPollAsync(chatId: chatId.ChatId, question: "Why are you nice ?", answers, isAnonymous: false);
                    }
                    else if (e.Update.PollAnswer.OptionIds[0] == 1)
                    {
                        List<string> answers = new List<string>();
                        answers.Add("Bye");
                        answers.Add("I`m Nice !");
                        var id = await botClient.SendPollAsync(chatId: chatId.ChatId, question: "Why are you bad ?", answers, isAnonymous: false);
                    }
                }
            }
        }
        private async void BotClient_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            Console.WriteLine(e.Message.Poll != null ? e.Message.Poll.TotalVoterCount : "");
            if (e.Message.Text != null)
            {
                switch (e.Message.Text.ToLower())
                {
                    case "hello":
                        {
                            List<string> answers = new List<string>();
                            answers.Add("Nice");
                            answers.Add("Bad");
                            var id = await botClient.SendPollAsync(chatId: e.Message.Chat, question: "How are you ?", answers, isAnonymous: false);
                            Polls.Add(new Poll
                            {
                                ChatId = e.Message.Chat,
                                PollId = id.Poll.Id
                            });
                            break;
                        }
                    case "📊 start quiz":
                        {
                            List<string> answers = new List<string>();
                            answers.Add("Nice");
                            answers.Add("Bad");
                            var id = await botClient.SendPollAsync(chatId: e.Message.Chat, question: "How are you ?", answers, isAnonymous: false);
                            Polls.Add(new Poll
                            {
                                ChatId = e.Message.Chat,
                                PollId = id.Poll.Id
                            });
                            break;
                        }
                    case "/start":
                        {
                            var keyboard = new ReplyKeyboardMarkup
                            {
                                Keyboard = new KeyboardButton[][]
                                {
                                    new KeyboardButton[]
                                    {
                                        new KeyboardButton("📊 Start quiz"),
                                        new KeyboardButton("📊 Start quiz"),
                                        new KeyboardButton("📊 Start quiz")
                                    },
                                    new KeyboardButton[]
                                    {
                                        new KeyboardButton("📊 Start quiz"),
                                        new KeyboardButton("📊 Start quiz")
                                    }
                                }
                            };
                            await botClient.SendStickerAsync(
                                chatId: e.Message.Chat,
                                sticker: "https://fs13.fex.net:443/download/2521380597",
                                replyMarkup: keyboard
                                );
                            break;
                        }
                }
            }
        }
    }
}
