using System;
using System.Collections.Generic;
using System.Text;

namespace MyWeatherApplication.VO
{
    /// <summary>
    /// This class creates model for weather response
    /// </summary>
    class WeatherResponse
    {
        #region Private members
        private DateTime _time;
        private double _temperature;
        private double _humidity;
        private string _weatherCondition;

        #endregion

        #region Public propeties
        public DateTime Time
        {
            get { return _time; }
            set { _time = value; }
        }
        public double Temperature
        {
            get { return _temperature; }
            set { _temperature = value; }
        }

        public double Humidity
        {
            get { return _humidity; }
            set { _humidity = value; }
        }

        public string WeatherCondition
        {
            get { return _weatherCondition; }
            set { _weatherCondition = value; }
        }
    }
        #endregion

    class DayWeatherReport : WeatherResponse
    {
        #region Private members
        private double _minTemperature;
        private double _maxTemperature;
        #endregion

        #region Public members
        public double MinTemperature
        {
            get { return _minTemperature; }
            set { _minTemperature = value; }
        }

        public double MaxTemperature
        {
            get { return _maxTemperature; }
            set { _maxTemperature = value; }
        }
        #endregion
    }
}
