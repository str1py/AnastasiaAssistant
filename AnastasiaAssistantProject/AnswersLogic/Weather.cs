using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

namespace AnastasiaAssistantProject.AnswersLogic
{
    class Weather
    {
        static string Osadki;
        static string Cloudiness;
        static string temp;
        static string Wind;
        static string WeatherState;
        static string City;
        static string CityToRequest;
        static string TimeOfDay;

        public static void GetTimeOfDay()
        {
            DateTime time = DateTime.Now;
            if (time.Hour > 4 && time.Hour < 12)
                TimeOfDay = "Утро";
            else if (time.Hour >= 12 && time.Hour < 18)
                TimeOfDay = "День";
            else if (time.Hour >= 18 && time.Hour < 24)
                TimeOfDay = "Вeчер";
            else if (time.Hour >= 0 && time.Hour < 4)
                TimeOfDay = "Ночь";

        }
        public static void SetWeatherAnswer()
        {
            DateTime time = DateTime.Now;
            string DateNow = time.Date.ToShortDateString();
            GetTimeOfDay();
            switch (WeatherState)
            {
                case "Слабый дождь":
                    MainAppLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", "+ TimeOfDay + "\nСостояние погоды" + WeatherState + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind + "\nОблачность: " + Cloudiness;
                    break;
                case "Ливневый дождь":
                    MainAppLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", " + TimeOfDay + "\nСостояние погоды" + WeatherState + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind + "\nОблачность: " + Cloudiness;
                    break;
                default:
                    MainAppLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", " + TimeOfDay + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind;
                    break;
            }
            switch (Cloudiness)
            {
                case "пасмурно(без просветов)":
                    MainAppLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", " + TimeOfDay + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind + "\nОблачность: " + Cloudiness;
                    break;
                case "значительная облачность":
                    MainAppLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", " + TimeOfDay + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind + "\nОблачность: " + Cloudiness;
                    break;
                case "малооблачно":
                    MainAppLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", " + TimeOfDay + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind + "\nОблачность: " + Cloudiness;
                    break;
                case "Дымка":
                    MainAppLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", " + TimeOfDay + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind + "\nОблачность: " + Cloudiness;
                    break;
                case null:
                    MainAppLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", " + TimeOfDay + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind;
                    break;
                default:
                    MainAppLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", " + TimeOfDay + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind;
                    break;
            }       
            CityToRequest = null;
        }

        public static void GetWeather()
        {       
            WebRequest request;
            if (MainAppLogic.MainVars.UserCommand.Contains("москва") || MainAppLogic.MainVars.UserCommand.Contains("москве"))
            {
                City = "Москва";
                CityToRequest = "moskva";
            }

            if (MainAppLogic.MainVars.UserCommand.Contains("питер") || MainAppLogic.MainVars.UserCommand.Contains("питере") ||
                MainAppLogic.MainVars.UserCommand.Contains("санкт-петербург") || MainAppLogic.MainVars.UserCommand.Contains("санкт-петербурге"))
            {
                City = "Cанкт-Петербург";
                CityToRequest = "sankt-peterburg";            
            }

            if(String.IsNullOrEmpty(CityToRequest)==true)
            {
                MainAppLogic.MainVars.AnastasiaAnswer = "Город не найден =(";
            }
            else
            {
                request = WebRequest.Create(@"http://www.meteoservice.ru/weather/now/" + CityToRequest + ".html");
                var response = request.GetResponse();
                var stream = response.GetResponseStream();
                var reader = new StreamReader(stream);
                string data = reader.ReadToEnd();
                temp = new Regex(@"<td class=""title"">Температура воздуха:</td>[^<]*?<td>(?<temp>[^<]+)&").Match(data).Groups["temp"].Value;
                Cloudiness = new Regex(@"<td class=""title"">Облачность:</td>[^<]*?<td>(?<Cloudiness>[^<]+)</td>").Match(data).Groups["Cloudiness"].Value;
                Osadki = new Regex(@"<td class=""title"">Количество осадков:</td>[^<]*?<td>(?<Osadki>[^<]+)</td>").Match(data).Groups["Osadki"].Value;
                Wind = new Regex(@"<td class=""title"">Ветер:</td>[^<]*?<td>(?<Wind>[^<]+)</td>").Match(data).Groups["Wind"].Value;
                WeatherState = new Regex(@"<td class=""title"" width=""40 % "">Состояние погоды:</td><td>WeatherState</td>").Match(data).Groups["WeatherState"].Value;
                SetWeatherAnswer();
            }

        }
    }
}
