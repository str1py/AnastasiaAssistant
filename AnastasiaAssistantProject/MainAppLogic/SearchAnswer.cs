using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace AnastasiaAssistantProject.MainAppLogic
{
    class SearchAnswer
    {

   
        AnswersLogic.TempSelect select = new AnswersLogic.TempSelect();
        AnswersLogic.Templates.SimpleAnswer SAData = new AnswersLogic.Templates.SimpleAnswer();
    
        public static void InitCommandList()
        {
            CommandList main = new CommandList();
            main.AddComands();
        }
        public static int LevenshteinDistance(string string1, string string2)
        {
            if (string1 == null) throw new ArgumentNullException("string1");
            if (string2 == null) throw new ArgumentNullException("string2");
            int diff;
            int[,] m = new int[string1.Length + 1, string2.Length + 1];

            for (int i = 0; i <= string1.Length; i++) { m[i, 0] = i; }
            for (int j = 0; j <= string2.Length; j++) { m[0, j] = j; }

            for (int i = 1; i <= string1.Length; i++)
            {
                for (int j = 1; j <= string2.Length; j++)
                {
                    diff = (string1[i - 1] == string2[j - 1]) ? 0 : 1;

                    m[i, j] = Math.Min(Math.Min(m[i - 1, j] + 1,
                                             m[i, j - 1] + 1),
                                             m[i - 1, j - 1] + diff);
                }
            }
            return m[string1.Length, string2.Length];
        }
        private void PlayMessageSound()
        {
            SoundPlayer sp = new SoundPlayer(Properties.Resources.IncomMessage);
            sp.Play();
        }


        public void SearchCommandInDic()
        {
  
            bool CommandHasFound = false;
            string commandToAction = null;
            string[] words=null;
            if (MainVars.UserCommand.Length > 4)
            {
                MainVars.UserCommand.Trim();
                int wordscount = 1;
                char[] command = MainVars.UserCommand.ToCharArray();
                for (int i = 0; i < MainVars.UserCommand.Length; i++) { if (command[i] == ' ') { wordscount++; } }//количество слов
                words = MainVars.UserCommand.Split(' ');//слова которые содержаться в запросе

                switch (wordscount)
                {
                    case 1:
                        commandToAction = words[0];
                        break;
                    case 2:
                        commandToAction = words[0] + " " + words[1];
                        break;
                    case 3:
                        commandToAction = words[0] + " " + words[1] + " " + words[2];
                        break;
                }


                foreach (KeyValuePair<string, Action> keyValue in CommandList.DicCommands)// Ищем целую команду с погрешностью < 3
                {
                    string commandFromDic = keyValue.Key.ToString();
                    if (LevenshteinDistance(commandToAction, commandFromDic) < 3)
                    {
                        MainVars.CommandToAction = commandFromDic;
                        if (CommandList.DicCommands.ContainsKey(MainVars.CommandToAction))
                        {
                            CommandList.DicCommands[MainVars.CommandToAction].Invoke();
                            MainWindow.Instance.UserMessage.Clear();
                            CommandHasFound = true;
                            PlayMessageSound();
                            break;
                        }
                    }
                }

                if (CommandHasFound == false)
                {
                    commandToAction = words[0];
                    foreach (KeyValuePair<string, Action> keyValue in CommandList.DicCommands)// Ищем команду по 1ому слову.
                    {
                        string commandFromDic = keyValue.Key.ToString();
                        if (LevenshteinDistance(commandToAction, commandFromDic) < 3)
                        {
                            MainVars.CommandToAction = commandFromDic;
                            if (CommandList.DicCommands.ContainsKey(MainVars.CommandToAction))
                            {
                                CommandList.DicCommands[MainVars.CommandToAction].Invoke();
                                MainWindow.Instance.UserMessage.Clear();
                                CommandHasFound = true;
                                PlayMessageSound();
                                break;
                            }
                        }
                    }
                }                    
            }
            else if(MainVars.UserCommand.Length <= 4)
            {
                if (CommandHasFound == false)
                {
                    commandToAction = MainVars.UserCommand;
                    foreach (KeyValuePair<string, Action> keyValue in CommandList.DicCommands)
                    {
                        string commandFromDic = keyValue.Key.ToString();
                        if (LevenshteinDistance(commandToAction, commandFromDic) ==0)
                        {
                            MainVars.CommandToAction = commandFromDic;
                            if (CommandList.DicCommands.ContainsKey(MainVars.CommandToAction))
                            {
                                CommandList.DicCommands[MainVars.CommandToAction].Invoke();
                                MainWindow.Instance.UserMessage.Clear();
                                CommandHasFound = true;
                                PlayMessageSound();
                                break;
                            }
                        }
                    }
                }
            }
            if (CommandHasFound == false)
            {
                MainVars.AnastasiaAnswer = "Не понимаю о чем вы...(((";
                select.SelectTemplate(SAData, null);
                MainWindow.Instance.UserMessage.Clear();
                PlayMessageSound();
            }
        }

    }
}
