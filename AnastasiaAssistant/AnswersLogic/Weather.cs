using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Media.Imaging;
using AnastasiaAssistant.AnswersLogic.Templates;

namespace AnastasiaAssistant.AnswersLogic
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

        public static void Morning()
        {
            DateTime time = DateTime.Now;
            string DateNow = time.Date.ToShortDateString();
            if (WeatherState.Contains("Слабый дождь"))
            {
                MainLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", Утро" + "\nСостояние погоды" + WeatherState + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind + "\nОблачность: " + Cloudiness;
            }
            else if (WeatherState.Contains("Ливневый дождь"))
            {
                MainLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", Утро" + "\nСостояние погоды" + WeatherState + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind + "\nОблачность: " + Cloudiness;
            }
            else
            {
                if (Cloudiness.Contains("пасмурно (без просветов)"))
                {
                    MainLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", Утро" + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind + "\nОблачность: " + Cloudiness;
                }
                else if (Cloudiness.Contains("значительная облачность") || Cloudiness.Contains("малооблачно") || Cloudiness.Contains("Дымка"))
                {
                    MainLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", Утро" + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind + "\nОблачность: " + Cloudiness;

                }
                else if (String.IsNullOrEmpty(Cloudiness))
                {
                    MainLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", Утро" + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind;
                    MainLogic.MainVars.GifPath = null;
                }
            }
            CityToRequest = null;
        }
        public static void Day()
        {
            DateTime time = DateTime.Now;
            string DateNow = time.Date.ToShortDateString();
            if (WeatherState.Contains("Слабый дождь"))
            {
                MainLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", День" + "\nСостояние погоды" + WeatherState + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind + "\nОблачность: " + Cloudiness;

            }
            else if (WeatherState.Contains("Ливневый дождь"))
            {
                MainLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", День"+ "\nСостояние погоды" + WeatherState + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind + "\nОблачность: " + Cloudiness;
            }
            else
            {
                if (Cloudiness.Contains("пасмурно (без просветов)"))
                {
                    MainLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", День" +"\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind + "\nОблачность: " + Cloudiness;
                    MainLogic.MainVars.GifPath = "/AnastasiaRes/Weather/Simple/cloudy.png";
                }
                if (Cloudiness.Contains("пасмурно (есть просветов)"))
                {
                    MainLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", День" + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind + "\nОблачность: " + Cloudiness;
                    MainLogic.MainVars.GifPath = "/AnastasiaRes/Weather/Simple/cloudy.png";
                }
                else if (Cloudiness.Contains("значительная облачность") || Cloudiness.Contains("малооблачно") || Cloudiness.Contains("Дымка"))
                {
                    MainLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", День"+ "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind + "\nОблачность: " + Cloudiness;
                }
                else if (String.IsNullOrEmpty(Cloudiness))
                {
                    MainLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", День" + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind ;
                    MainLogic.MainVars.GifPath = null;
                }
            }
            CityToRequest = null;
        }
        public static void Afternoon()
        {
            DateTime time = DateTime.Now;
            string DateNow = time.Date.ToShortDateString();
            if (WeatherState.Contains("Слабый дождь"))
            {
                MainLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", Вечер" + "\nСостояние погоды" + WeatherState + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind + "\nОблачность: " + Cloudiness;
                MainLogic.MainVars.GifPath = "/AnastasiaRes/Weather/Simple/rain.png";
            }
            else if (WeatherState.Contains("Ливневый дождь"))
            {
                MainLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", Вечер" + "\nСостояние погоды" + WeatherState + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind + "\nОблачность: " + Cloudiness;
                MainLogic.MainVars.GifPath = "/AnastasiaRes/Weather/Simple/rain.png";
            } else if (Cloudiness.Contains("пасмурно (без просветов)"))
            {
                MainLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", Вечер" + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind + "\nОблачность: " + Cloudiness;
                MainLogic.MainVars.GifPath = "/AnastasiaRes/Weather/Simple/cloudy.png";
            }
            if (Cloudiness.Contains("пасмурно (есть просветы)"))
            {
                MainLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", Вечер" + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind + "\nОблачность: " + Cloudiness;
                MainLogic.MainVars.GifPath = "/AnastasiaRes/Weather/Simple/cloudy.png";
            }
            else if (Cloudiness.Contains("значительная облачность") || Cloudiness.Contains("малооблачно") || Cloudiness.Contains("Дымка"))
            {
                MainLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", Вечер" + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind + "\nОблачность: " + Cloudiness;
                MainLogic.MainVars.GifPath = "/AnastasiaRes/Weather/Simple/cloudy.png";
            }
            else if (String.IsNullOrEmpty(Cloudiness) == true)
            {
                MainLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", Вечер" + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind;
                MainLogic.MainVars.GifPath = null;
            }
            else
            {
                MainLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", Вечер" + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind;
            }
            CityToRequest = null;
        }
        public static void Night()
        {
            DateTime time = DateTime.Now;
            string DateNow = time.Date.ToShortDateString();
            if (WeatherState.Contains("Слабый дождь"))
            {
                MainLogic.MainVars.AnastasiaAnswer = City+ " - " + "Сегодня," + DateNow + ", Ночь" + "\nСостояние погоды" + WeatherState + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind + "\nОблачность: " + Cloudiness;
                MainLogic.MainVars.GifPath = "/AnastasiaRes/Weather/Night/cattered_showers_night.png";
            }
            else if (WeatherState.Contains("Ливневый дождь"))
            {
                MainLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", Ночь" + "\nСостояние погоды" + WeatherState + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind + "\nОблачность: " + Cloudiness;
                MainLogic.MainVars.GifPath = "/AnastasiaRes/Weather/Night/HeavyRain.gif";
            }else if(WeatherState.Contains("Гроза слабая или умеренная в срок с дождем или снегом"))
            {
                MainLogic.MainVars.GifPath = "/AnastasiaRes/Weather/Night/rain_and_thunderstorms_night.png";
            }
            else
            {
                if (Cloudiness.Contains("пасмурно (без просветов)"))
                {
                    MainLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", Ночь" + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind + "\nОблачность: " + Cloudiness;
                      //
                }
                if (Cloudiness.Contains("пасмурно (есть просветы)"))
                {
                    MainLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", Ночь" + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind + "\nОблачность: " + Cloudiness;
                    MainLogic.MainVars.GifPath = "/AnastasiaRes/Weather/Night/cloudy_night.png";
                }
                else if (Cloudiness.Contains("значительная облачность") || Cloudiness.Contains("малооблачно") || Cloudiness.Contains("Дымка"))
                {
                    MainLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", Ночь" + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind + "\nОблачность: " + Cloudiness;
                    MainLogic.MainVars.GifPath = "/AnastasiaRes/Weather/Simple/cloudy.png";
                }
                else 
                {
                    MainLogic.MainVars.AnastasiaAnswer = City + " - " + "Сегодня," + DateNow + ", Ночь" + "\nКолличество осадков: " + Osadki + " " + "\nСейчас: " + temp + "°C" + ",  " + Wind;
                    MainLogic.MainVars.GifPath = "/AnastasiaRes/Weather/Night/night.png";
                }
            }
            CityToRequest = null;
        }
    
        public static void GetWeather()
        {       
            WebRequest request;
            if (MainLogic.MainVars.UserCommand.Contains("москва") || MainLogic.MainVars.UserCommand.Contains("москве"))
            {
                City = "Москва";
                CityToRequest = "moskva";
            }

            if (MainLogic.MainVars.UserCommand.Contains("питер") || MainLogic.MainVars.UserCommand.Contains("питере") || MainLogic.MainVars.UserCommand.Contains("санкт-петербург") || MainLogic.MainVars.UserCommand.Contains("санкт-петербурге"))
            {
                City = "Cанкт-Петербург";
                CityToRequest = "sankt-peterburg";            
            }

            if(String.IsNullOrEmpty(CityToRequest)==true)
            {
                MainLogic.MainVars.AnastasiaAnswer = "Город не найден =(";
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
                DateTime time = DateTime.Now;
                if (time.Hour > 4 && time.Hour < 12)
                    Morning();
                else if (time.Hour >= 12 && time.Hour < 18)
                    Day();
                else if (time.Hour >= 18 && time.Hour < 24)
                    Afternoon();
                else if (time.Hour >= 0 && time.Hour < 4)
                    Night();
            }

        }
    }
}
