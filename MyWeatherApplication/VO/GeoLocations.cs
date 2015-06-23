using System;
using System.Collections.Generic;
using System.Text;

namespace MyWeatherApplication.VO
{
    /// <summary>
    /// Class to hold latitude and longitude of device
    /// </summary>
    class GeoLocations
    {
        #region Private members
        
        private static double _latitude;
        private static double _longitude;

        #endregion

        #region Public properties
        /// <summary>
        /// Property to set or get device latitude
        /// </summary>
        public static double Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }

        /// <summary>
        /// Property to set or get device longitude
        /// </summary>
        public static double Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }
        #endregion
    }
}
