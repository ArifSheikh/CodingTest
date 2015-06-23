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
    /// Current Fragment for displaying current weather
    /// </summary>
    public class CurrentFragment : Fragment
    {
        private View mobjView = null;
        private TextView tvTime = null;
        private TextView tvTemperature = null;
        private TextView tvhumidity = null;
        private TextView tvWeatherConditions = null;
        WeatherResponse mobjCurrentWeather=null;
        private bool Visiblity = false;


        public CurrentFragment(bool Visibility)
        {
            Visiblity = Visibility;
        }
        /// <summary>
        /// Method to initialize all the resources
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        #region Overriden Methods
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
                if (Visiblity)
                {
                    mobjView = inflater.Inflate(Resource.Layout.CurrentWeather, container, false);
                    tvTime = mobjView.FindViewById<TextView>(Resource.Id.TimeText);
                    tvTemperature = mobjView.FindViewById<TextView>(Resource.Id.tempText);
                    tvhumidity = mobjView.FindViewById<TextView>(Resource.Id.humidText);
                    tvWeatherConditions = mobjView.FindViewById<TextView>(Resource.Id.condText);

                    mobjCurrentWeather = WeatherReport.GetCurrentInstance();

                    tvTime.Text = mobjCurrentWeather.Time.ToString(GlobalConstants.DateTimeFormat);
                    tvTemperature.Text = mobjCurrentWeather.Temperature.ToString() + GlobalConstants.FahrenheitKeyword;
                    tvhumidity.Text = mobjCurrentWeather.Humidity.ToString() + GlobalConstants.PercentageSymbol;
                    tvWeatherConditions.Text = mobjCurrentWeather.WeatherCondition;
                }
                return mobjView;
            }
            catch (Exception ex)
            {
                Toast.MakeText(Activity , GlobalConstants.ErrorMessage+ex.ToString(), ToastLength.Short);
            }
            return mobjView;
        }
        #endregion
    }
}