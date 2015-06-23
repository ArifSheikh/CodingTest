using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MyWeatherApplication.VO;

namespace MyWeatherApplication.UI
{
    /// <summary>
    /// Current Fragment for displaying hourly weather
    /// </summary>
    public class HourlyFragment : Fragment
    {
        private View view = null;
        private ListView hourlyWeatherList = null;

        #region Overriden Methods

        /// <summary>
        /// Method to initialize all the resources
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        /// <summary>
        /// Method to intialize all the elements for view creation
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                view = inflater.Inflate(Resource.Layout.HourlyWeather, container, false);
                hourlyWeatherList = view.FindViewById<ListView>(Resource.Id.lv_hourly_weather);
                HourlyFragmentAdapter hourlyAdapter = new HourlyFragmentAdapter(Activity, Resource.Layout.WeatherListItem, WeatherReport.GetHourlyInstance());
                hourlyWeatherList.Adapter = hourlyAdapter;
                return view;
            }
            catch (Exception ex)
            {
                Toast.MakeText(Activity, GlobalConstants.ErrorMessage + ex.ToString(), ToastLength.Short);
            }
            return view;
        }

        #endregion
    }
}