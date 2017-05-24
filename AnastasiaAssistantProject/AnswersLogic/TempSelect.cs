using AnastasiaAssistantProject.AnswersLogic.Templates;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AnastasiaAssistantProject.AnswersLogic
{
    class TempSelect : DataTemplateSelector
    {
        public DataTemplate SimpleAnswer { get; set; }
        public DataTemplate AnswerWithPict { get; set; }
        public DataTemplate Genres { get; set; }

        List<SimpleAnswer> SansData = new List<SimpleAnswer>();
        List<AnswerWithPict> WansData = new List<AnswerWithPict>();

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is SimpleAnswer)
            {
                SansData.Clear();
                SansData.Add(new SimpleAnswer()
                {
                    Answer = MainAppLogic.MainVars.AnastasiaAnswer
                });
                MainWindow.Instance.MainChat.ItemsSource = SansData;
                return SimpleAnswer;
            }
            else if (item is AnswerWithPict)
            {
                WansData.Clear();
                WansData.Add(new AnswerWithPict()
                {
                    Answer = MainAppLogic.MainVars.AnastasiaAnswer,
                    WeatherBack = MainAppLogic.MainVars.GifPath,
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
