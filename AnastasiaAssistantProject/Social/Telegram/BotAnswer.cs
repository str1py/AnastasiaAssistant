using System;
using System.Threading.Tasks;
using Telegram.Bot;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace AnastasiaAssistantProject.Social.Telegram
{
    class BotAnswer
    {
        private readonly TelegramBotClient Bot = new TelegramBotClient("245930967:AAEinicyxhw42E7DO_RApvk1fEbvBVxx-i4");
        BassPlayer.BassNetHelper bass = new BassPlayer.BassNetHelper();
        List<AnswersLogic.Templates.SimpleAnswer> SADATA = new List<AnswersLogic.Templates.SimpleAnswer>();
        AnswersLogic.TempSelect select = new AnswersLogic.TempSelect();
        string callbackNum;

        public void Main()
        {
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnMessageEdited += BotOnMessageReceived;
            var me = Bot.GetMeAsync().Result;
            Bot.StartReceiving();
        }
        public void StopRec()
        {
            try
            {
                Bot.StopReceiving();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void TestMessage()
        {
            Bot.SendTextMessageAsync(Properties.Settings.Default.TelegramUserID, "Интерграция с Telegram завершена!");
            //  Bot.SendStickerAsync(Properties.Settings.Default.TelegramID, "/Resources/Sticker.png");
        }
        private async void BotClickPlayerButtons()
        {
            if (callbackNum == "1")
            {
                await MainWindow.Instance.TabMusicPrev.Dispatcher.BeginInvoke(new Action(delegate ()
                {
                    bass.PreviousSongButton();           
                }));
            }
            else if (callbackNum == "2")
            {
                await MainWindow.Instance.TabMusicPlay.Dispatcher.BeginInvoke(new Action(delegate ()
                {
                    bass.PlayButton();
                }));
            }
            else if (callbackNum == "3")
            {
                await MainWindow.Instance.TabMusicPause.Dispatcher.BeginInvoke(new Action(delegate ()
                {
                    bass.StopButton();
                }));
            }
            else
            {
                await MainWindow.Instance.TabMusicNext.Dispatcher.BeginInvoke(new Action(delegate ()
                {
                    bass.NextSongButton();
                }));
            }
        }
        private async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            if (message == null || message.Type != MessageType.TextMessage) return;
            await Bot.SendChatActionAsync(Properties.Settings.Default.TelegramUserID, ChatAction.Typing);

            var answer = message.Text; //Cообщение от пользователя 

            await MainWindow.Instance.UserMessage.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                MainWindow.Instance.UserMessage.Text = answer;
            }));       
            await MainWindow.Instance.SendButt.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                MainWindow.Instance.SendButt_Click(MainWindow.Instance, new System.Windows.RoutedEventArgs()); //Нажатие кнопки отпавки              
            }));
            await Task.Delay(500);
            await MainWindow.Instance.SendButt.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                Bot.SendTextMessageAsync(Properties.Settings.Default.TelegramUserID, MainAppLogic.MainVars.AnastasiaAnswer);//получение ответа и отправка ответа в TELEGRAM
            }));
        }
        private async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            callbackNum = callbackQueryEventArgs.CallbackQuery.Data;
            BotClickPlayerButtons();
            await Task.Delay(500);
            await Bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id,
              $" {MainAppLogic.MainVars.SongName}");
        }
        public async void ShowPlayerButtons(object sender, MessageEventArgs messageargs)
        {
            await Bot.SendChatActionAsync(Properties.Settings.Default.TelegramUserID, ChatAction.Typing);
            var keyboard = new InlineKeyboardMarkup(new[]
                {
                    new[] // first row
                    {
                        new InlineKeyboardButton("Prev","1"),
                        new InlineKeyboardButton("Play","2"),
                        new InlineKeyboardButton("Pause","3"),
                        new InlineKeyboardButton("Next","4"),
                    }
                });
            await Bot.SendTextMessageAsync(Properties.Settings.Default.TelegramUserID, "Управление плеером",
            replyMarkup: keyboard);
        }
    }
}
