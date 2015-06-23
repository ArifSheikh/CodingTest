using System;
using System.Collections.Generic;
using System.Text;

namespace MyWeatherApplication
{
    /// <summary>
    /// Creating global Exception
    /// </summary>
    class GlobalConstants
    {
        public  const string WeatherForecastAPIUrl = "https://api.forecast.io/forecast";
        public  const string APIKey = "96bcdcdacfd563547aac297208354aa3";
        public  const char Separator = '/';
        public const string DateTimeFormat = "ddd, dd MMM yyyy HH:mm:ss";
        public const char PercentageSymbol = '%';
        public const char FahrenheitKeyword = 'F';
        public const string ErrorMessage = "Error Occured";
    }
}
