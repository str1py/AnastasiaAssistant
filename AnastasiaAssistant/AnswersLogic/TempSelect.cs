using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AnastasiaAssistant.AnswersLogic
{
    class TempSelect : DataTemplateSelector
    {
        public DataTemplate SimpleAnswer { get; set; }
        public DataTemplate AnswerWithPict { get; set; }
        public DataTemplate Genres { get; set; }

        List<Templates.SimpleAnswer> SansData = new List<Templates.SimpleAnswer>();
        List<Templates.AnswerWithPict> WansData = new List<Templates.AnswerWithPict>();

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is AnastasiaAssistant.AnswersLogic.Templates.SimpleAnswer)
            {
                SansData.Clear();
                SansData.Add(new AnswersLogic.Templates.SimpleAnswer()
                {
                    Answer = MainLogic.MainVars.AnastasiaAnswer
                });
                MainWindow.Instance.MainChat.ItemsSource = SansData;
                return SimpleAnswer;
            }
            else if (item is AnastasiaAssistant.AnswersLogic.Templates.AnswerWithPict)
            {
                WansData.Clear();
                WansData.Add(new AnswersLogic.Templates.AnswerWithPict()
                {
                    Answer = MainLogic.MainVars.AnastasiaAnswer,
                    WeatherBack = MainLogic.MainVars.GifPath,
                });
                MainWindow.Instance.MainChat.ItemsSource = WansData;
                return AnswerWithPict;
            }
         
            else
            {
                return null;
            }
        }
    }


}
