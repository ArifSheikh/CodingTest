using System;
using System.Collections.Generic;
using System.Text;

namespace MyWeatherApplication.VO
{
    /// <summary>
    /// Class to implement singleton pattern
    /// </summary>
    class WeatherReport
    {
        #region Private Members

        /// <summary>
        /// 
        /// </summary>
        private static WeatherResponse _currentWeather = null;
        private static List<WeatherResponse> _HourlyWeather = null;
        private static List<DayWeatherReport> _DailyWeather = null;
        
        #endregion

        #region Public Members
        /// <summary>
        /// Gets the current instance of this class
        /// </summary>
        /// <returns></returns>
        public static WeatherResponse GetCurrentInstance()
        {
            if (_currentWeather == null)
            {

                if (_currentWeather == null)
                {
                    _currentWeather = new WeatherResponse();
                }
            }
            return _currentWeather;
        }

        /// <summary>
        /// Gets the current instance of this class
        /// </summary>
        /// <returns></returns>
        public static List<WeatherResponse> GetHourlyInstance()
        {
            if (_HourlyWeather == null)
            {

                if (_HourlyWeather == null)
                {
                    _HourlyWeather = new List<WeatherResponse>();
                }
            }
            return _HourlyWeather;
        }

        /// <summary>
        /// Gets the current instance of this class
        /// </summary>
        /// <returns></returns>
        public static List<DayWeatherReport> GetDailynstance()
        {
            if (_DailyWeather == null)
            {

                if (_DailyWeather == null)
                {
                    _DailyWeather = new List<DayWeatherReport>();
                }
            }
            return _DailyWeather;
        }
        
        #endregion

    }
}
